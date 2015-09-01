using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseController : MonoBehaviour {

	

	// Stuff from old HA
	// maxSpeed is now desiredSpeed
	public float startMaxSpeed;
	public float middleMaxSpeed;
	public float endMaxSpeed;
	
	public Vector3 lastServerPosition;
	public Vector3 lastLocalPosition;
		
	public int inputState;
	
	public float jumpSpeed;
	public float behindBoost;
	public float infrontBoost;
	
	public float whipEffect = 1.05f;
	public float easeEffect = 0.9f;

	public float totalRaceDistance;
	
	public bool trainingRun = false;
	
	public int ownerID;
	
	public int position;
	public int finishPosition;

	//
	public Animator animator;
	public float lastRouteRequest;
	public RacingLine myRacingLine;

	public RaceLinePoint currentPoint;
	public RaceLinePoint prevPoint;

	public float closenessToNextPoint;
	public float speed = 4f;
	public float desiredSpeed = 6f;
	public float rotationSpeed = 1f;
	public float acceleration = 0.1f;
	public float deceleration = 0.1f;
	public const float MIN_FOLLOW_DISTANCE = 8f;
	public Transform horseNose;
	private Quaternion _lookRotation;
	private float _lastLookMagnitude;
	private Vector3 _direction;
	public bool useMidway = false;
	public bool hasCamera = false;
	public bool hasFinished = false;

	public bool hasStarted = false;
	// Use this for initialization
	void Start () {
		findStartRaceLine();
		horseNose = this.transform.FindChild("HorseNose");
	}

	public void OnTriggerEnter(Collider aCollision) {
		if(!hasFinished&&aCollision.GetComponent<Collider>().gameObject.tag=="Finish") {
			hasFinished = true;
		}
	}
	void findStartRaceLine() {
		GameObject[] g = GameObject.FindGameObjectsWithTag("RacingLine");

		RacingLine closest = g[0].GetComponent<RacingLine>();
		float closestDist = float.MaxValue;
		for(int i = 0;i<g.Length;i++) {
			RacingLine r = g[i].GetComponent<RacingLine>();
			float dist = r.getDistanceFromHorse(this.transform.position);
			if(dist<closestDist) {
				closestDist = dist;
				closest = r;
			}
		}
		myRacingLine = closest;
		

	}

	public float timeUntilPointAtCurrentSpeed {
		get {
			float dist = Vector3.Distance(this.transform.position,this.currentPoint.transform.position);
			return dist / this.speed;
		}
	}
	public float timeUntilPointAtDesiredSpeed {
		get {
			float dist = Vector3.Distance(this.transform.position,this.currentPoint.transform.position);
			return dist / this.desiredSpeed;
		}
	} 
	public HorseController horseInFront {
		get {
			RaycastHit hit;
			Vector3 there = this.currentPoint.gameObject.transform.position;
			Vector3 here = this.transform.position;
			there.y = this.transform.position.y;
			here.y = there.y;
			
			Debug.DrawRay(here,this.transform.forward);
			if(Physics.SphereCast(horseNose.position,1f,this.transform.forward,out hit)||Physics.Linecast(here,there,out hit)) {
				if(hit.collider.gameObject.tag=="Player") {
					HorseController h = hit.collider.gameObject.GetComponent<HorseController>();
					if(!h.hasFinished&&h.myRacingLine==this.myRacingLine)
						return h;
				}
			}
			return null;
		}
	}	
	private void handleFinishedHorse() {
		if(this.speed>0f) {
			speed -= this.deceleration/4f;
			if(speed<0f) {
				speed = 0f;
			}
		}
		float step = speed * Time.deltaTime;

		
		
		

		//create the rotation we need to be in to look at the target
//		_lookRotation = Quaternion.LookRotation(_direction);
//		Quaternion r = transform.rotation;
//		transform.rotation = Quaternion.Slerp(r, _lookRotation, Time.deltaTime * this.rotationSpeed);
		
		Vector3 diff = transform.forward*step;
		transform.position += diff;
		this.animator.SetFloat("Speed",speed);
		
	}
	// Update is called once per frame
	void FixedUpdate () {
		if(!this.hasStarted) {
			return;
		}
		if(this.hasFinished) {
			handleFinishedHorse();
			return;
		}
		if(currentPoint==null) {
			prevPoint = currentPoint;
			currentPoint = myRacingLine.getClosestNodeToHorse(this.transform.position);
		}
		bool blocked = false;
		HorseController infront = this.horseInFront;
		Debug.DrawRay(horseNose.position,this.transform.forward,Color.yellow);
		Debug.DrawLine(this.transform.position,currentPoint.transform.position);
		if(Vector3.Distance(currentPoint.transform.position,this.transform.position)<closenessToNextPoint) {
			// We're looking ahead forward for the next race point now, lets decide how to deal with it.
			bool changingRaceLines = false;
			prevPoint = currentPoint;
			currentPoint = myRacingLine.getNextPoint(currentPoint);
			useMidway = true;
		//	this.desiredSpeed += UnityEngine.Random.Range(-1f,1f);
			if(desiredSpeed<9) {
				desiredSpeed =9f;
			}
			changingRaceLines = findAltRacingLine(true);
			if(!changingRaceLines) {
				// Find out whether there is a horse between us and the next point
				
				if(infront!=null) {
					if(this.currentPoint==infront.currentPoint) {
						float timeUntilTheyAtPoint = infront.timeUntilPointAtCurrentSpeed;
						float timeUntilWeAtPoint = this.timeUntilPointAtDesiredSpeed;
						if(timeUntilTheyAtPoint>timeUntilWeAtPoint) {
							this.findAltRacingLine(false);
						}
					}
				} 
			}
		} else {
			if(infront!=null) {
				float dist = Vector3.Distance(infront.transform.position,this.transform.position);

				if(dist<MIN_FOLLOW_DISTANCE) {
					if(this.speed>infront.speed) {
						speed = infront.speed-0.1f;
						blocked = true;
					}
				}
			}
		}

		if(!blocked) {
			if(speed<desiredSpeed) {
				speed += acceleration;
			} else if(speed>desiredSpeed) {
				speed -= this.deceleration;
			}
		}
		float step = speed * Time.deltaTime;
		Vector3 endPlace = currentPoint.transform.position;
		
		if(useMidway&&prevPoint!=null) {
			endPlace = (currentPoint.transform.position-prevPoint.transform.position)/2+prevPoint.transform.position;
		
			Debug.DrawLine(endPlace,this.transform.position,Color.cyan);
		}

		

		//find the vector pointing from our position to the target
		_direction = (endPlace - transform.position).normalized;
		
		//create the rotation we need to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);
		Quaternion r = transform.rotation;
		transform.rotation = Quaternion.Slerp(r, _lookRotation, Time.deltaTime * this.rotationSpeed);

		Vector3 diff = transform.forward*step;
		transform.position += diff;

		if(this.useMidway) {
			float distFromStart = Vector3.Distance(this.horseNose.transform.position,this.prevPoint.transform.position);
			float distFromFinish = Vector3.Distance(this.horseNose.transform.position,this.currentPoint.transform.position);
			if(distFromStart*1.25f>distFromFinish) {
				useMidway = false;
			}
		}
		this.animator.SetFloat("Speed",speed);

		
	}
	private HorseController horseInTargetLaneAtVector(Vector3 aHere,Vector3 aThere,RacingLine aLine) {
		RaycastHit hit;
		Debug.DrawLine(aHere,aThere);
		if(Physics.Linecast(aHere,aThere,out hit)) {
			if(hit.collider.gameObject.tag=="Player") {
				HorseController h = hit.collider.GetComponent<HorseController>();
				if(h.myRacingLine==aLine) {
					return h;
				}
			}
		}
		return null;
	}
	private bool findAltRacingLine(bool aShorterOnly) {
		List<RacingLine> allRacingLines = RaceTrack.REF.racingLines;
		for(int i = 0;i<allRacingLines.Count;i++) {
			bool a = (i>0&&allRacingLines[i-1]==this.myRacingLine);
			bool b = (i<allRacingLines.Count-1&&allRacingLines[i+1]==this.myRacingLine);
			if(a||b) {
				RaceLinePoint r = allRacingLines[i].pointAtIndex(currentPoint.thisIndex);
				float currentDistanceToFinish = this.currentPoint.distanceToFinish+(Vector3.Distance(this.transform.position,this.currentPoint.transform.position));
				float altLineDistanceToFinish =	r.distanceToFinish+(Vector3.Distance(this.transform.position,r.transform.position));
				bool shorter = altLineDistanceToFinish<currentDistanceToFinish;
				
				if(!aShorterOnly||altLineDistanceToFinish<currentDistanceToFinish) {
					// We want to move to a different race line if we can
					
					RaceLinePoint p = allRacingLines[i].pointAtIndex(currentPoint.thisIndex-1);
					Vector3 there = p.transform.position;
					
					Vector3 diff = p.transform.position-this.transform.position;
					float dist = Vector3.Distance(allRacingLines[i].pointAtIndex(currentPoint.thisIndex-1).transform.position,this.transform.position);
					
					
					// Spherecast towards the current point on this line, if there is something there, lets
					RaycastHit hit;
					
					float distanceToObstacle = 0;
					
					// Cast a sphere wrapping character controller 10 meters forward
					// to see if it is about to hit anything.
					Vector3 here = this.transform.position;
					here.y += 1f;
					there.y += 1f;
					Vector3 left = (this.transform.right*-1)*(Vector3.Distance(here,there))+this.transform.position;
					Vector3 right = (this.transform.right)*(Vector3.Distance(here,there))+this.transform.position;
					left.y+=1f;
					right.y+=1f;
					// I don't know whether this is on left or right of us right now, lets see whats closest to "There"
					Vector3 sideways = right;
					if(Vector3.Distance(left,there)<Vector3.Distance(right,there)) {
						sideways = left;
					}
					Vector3 infrontAndLeftDiff = there-sideways;
					Vector3 behind = sideways-infrontAndLeftDiff;
					Vector3 behindDouble = sideways-infrontAndLeftDiff*3;
					Vector3 infrontAndSide = sideways+infrontAndLeftDiff/2;


					if((this.horseInTargetLaneAtVector(here,there,allRacingLines[i])!=null)||
					   (this.horseInTargetLaneAtVector(here,sideways,allRacingLines[i])!=null)||
					   (this.horseInTargetLaneAtVector(here,behind,allRacingLines[i])!=null)||
					   (this.horseInTargetLaneAtVector(here,infrontAndSide,allRacingLines[i])!=null)||
					   (this.horseInTargetLaneAtVector(here,behindDouble,allRacingLines[i])!=null)) {
	
					} else {
						if(this.hasCamera)
								Debug.Log ("Found a new racing line, new distance: "+altLineDistanceToFinish+" - Old Distance: "+currentDistanceToFinish+" Looking for shorter routes only: "+aShorterOnly);
						this.myRacingLine = allRacingLines[i];
						if(currentPoint.thisIndex>0)
							prevPoint = allRacingLines[i].pointAtIndex(currentPoint.thisIndex-1);
						this.currentPoint = allRacingLines[i].pointAtIndex(currentPoint.thisIndex);
						useMidway = true;
						return true;
					}
					
					
				}
			}
		}
		// No suitable race line found
		return false;
	}
}
