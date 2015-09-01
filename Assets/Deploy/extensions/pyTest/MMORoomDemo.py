# ~~~ Java Imports ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

from com.smartfoxserver.v2.core import ISFSEventListener
from com.smartfoxserver.v2.core import SFSEventType
from com.smartfoxserver.v2.core import SFSEventParam
from com.smartfoxserver.v2.core import *
from com.smartfoxserver.v2.exceptions import *
from com.smartfoxserver.v2.entities import *
from com.smartfoxserver.v2.entities.data import *
from com.smartfoxserver.v2.entities.variables import *
from com.smartfoxserver.v2.entities.variables import SFSUserVariable
from com.smartfoxserver.bitswarm.sessions import *
from com.smartfoxserver.v2.api import *
from com.smartfoxserver.v2.util import *
from com.smartfoxserver.v2.mmo import *
from com.smartfoxserver.v2.mmo import Vec3D
from com.smartfoxserver.v2.security import DefaultPermissionProfile
from com.smartfoxserver.v2 import SmartFoxServer

from java.lang import *
from java.util.concurrent import TimeUnit

# ~~~ Python Imports ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

from random import randint, uniform

# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

VERSION = "1.02"
userVarsListener = None
mmoApi = None

MAX_MAP_X = 100
MAX_MAP_Z = 100
MAX_NPC = 50

npcList = None
allScheduledTasks = None		
	
#
# On every client's UserVariableUpdate we also invoke a SetUserPosition
# to update the MMORoom about the client's position in the map
#
class UserVarsUpdateListener(ISFSEventListener):
	def handleServerEvent(self, event):
		variables = event.getParameter(SFSEventParam.VARIABLES)
		user = event.getParameter(SFSEventParam.USER)

		# Make a map of the variables list
		varMap = {}
		for var in variables:
			varMap[var.name] = var

		if varMap.has_key('x') and varMap.has_key('z'):
			pos = Vec3D(Float(varMap['x'].value), Float(1.0), Float(varMap['z'].value))
			mmoApi.setUserPosition(user, pos, _base.getParentRoom())

#
# Moves every NPC randomly in the system
#
class NpcRunner(Runnable):
	def run(self):
		userList = _base.getParentRoom().getUserList()		

		for user in userList:
			if user.isNpc():
				xspeed = user.getProperty('xspeed')
				zspeed = user.getProperty('zspeed')
				
				xpos = user.getVariable('x').getDoubleValue()
				zpos = user.getVariable('z').getDoubleValue()
				
				newX = xpos + xspeed 
				newZ = zpos + zspeed
				
				# Check Map X limits
				if newX < -100 or newX > 100:
					newX = xpos
					xspeed *= -1
					user.setProperty('xspeed', xspeed)
				
				# Check Map Z limits
				if newZ < -100 or newZ > 100:
					newZ = zpos
					zspeed *= -1
					user.setProperty('zspeed', zspeed)
			
				_sfsApi.setUserVariables(user, [SFSUserVariable('x', newX), SFSUserVariable('z', newZ)])
				
				
## --------------------------------------------------------------------------------------------##
		
def init():	
	global mmoApi, sfsEventListener, userVarsListener	
	trace("MMO Item Tester: ", VERSION)
		
	userVarsListener = UserVarsUpdateListener()
	_base.addEventListener(SFSEventType.USER_VARIABLES_UPDATE, userVarsListener)

	mmoApi = SmartFoxServer.getInstance().getAPIManager().getMMOApi()
	
	# Start simulated clients
	simulatePlayers()

	
def destroy():
	_base.removeEventListener(SFSEventType.USER_VARIABLES_UPDATE, userVarsListener)

	trace("Python extension destroyed: ", _base)


## --------------------------------------------------------------------------------------------##
## --------------------------------------------------------------------------------------------##

def simulatePlayers():
	global npcList, allScheduledTasks
	
	npcList = []
	allScheduledTasks = []
	mmoRoom = _base.getParentRoom()
	
	# Generate users
	for ii in xrange(0, MAX_NPC):
		npcUser = _sfsApi.createNPC("NPC#"+str(ii), _base.getParentZone(), False)
		npcList.append(npcUser)
			
	for user in npcList:
		rndX = randint(0, MAX_MAP_X)
		rndY = 1
		rndZ = randint(0, MAX_MAP_Z)
		
		if randint(0,100) > 49:
			rndX *= -1
			
		if randint(0,100) > 49:
			rndZ *= -1
		
		rndPos = Vec3D(Float(rndX), Float(1.0), Float(rndZ))
		
		uVars = [
			SFSUserVariable("x", Double(rndX)),
			SFSUserVariable("y", Double(rndY)),
			SFSUserVariable("z", Double(rndZ)),
			SFSUserVariable("rot", Double(randint(0,360))),
			SFSUserVariable("model", 2),
			SFSUserVariable("mat", randint(0,3))
		]
		
		user.setProperty("npcData", {"isMoving":False, "oldPos":None, "msgCount": 0})
		user.setProperty("xspeed", getRandomSpeedValue())
		user.setProperty("zspeed", getRandomSpeedValue())
		
		# Set Vars
		_sfsApi.setUserVariables(user, uVars, False, False)
		
		# Join Room
		_sfsApi.joinRoom(user, mmoRoom)
		
		# Set User Position
		mmoApi.setUserPosition(user, rndPos , mmoRoom)	
		

	taskHandle = _sfs.getTaskScheduler().scheduleAtFixedRate(NpcRunner(), 0, 100, TimeUnit.MILLISECONDS)
		#allScheduledTasks.append(taskHandle)


def getRandomSpeedValue():
	value = uniform(0, 1.2)
	
	if (randint(0,100) > 49):
		value *= -1

	return value

## --------------------------------------------------------------------------------------------##
## --------------------------------------------------------------------------------------------##		

	
def handleClientRequest(cmd, sender, params):
	pass
	
def handleInternalMessage(cmd, param):
	pass
	
## --------------------------------------------------------------------------------------------##
