using System.Collections;
using UnityEngine;

namespace OrX
{
	public class OrXTargetInfo : MonoBehaviour
	{
		public OrXHoloCache.OrXCoords OrbitalBodyName;
        public float detectedTime;

        public float radarBaseSignature = -1;
        public bool radarBaseSignatureNeedsUpdate = true;
        public float radarModifiedSignature;
        public float radarLockbreakFactor;
        public float radarJammingDistance;
        public bool alreadyScheduledRCSUpdate = false;


        public bool isLanded
		{
			get
			{
                if (!vessel) return false;
                if (
                    (vessel.situation == Vessel.Situations.LANDED ||
                    vessel.situation == Vessel.Situations.SPLASHED) && // Boats should be included 
                    !isUnderwater
                    )
                {
                    
                    return true;
                }
                
                else
                    return false;
            }
		}

        public bool isFlying
        {
            get
            {
                if (!vessel) return false;
                if (vessel.situation == Vessel.Situations.FLYING || vessel.situation == Vessel.Situations.ORBITING) return true;
                else
                    return false;
            }

        }

        public bool isUnderwater
        {
            get
            {
                if (!vessel) return false;
                if (vessel.altitude < -20) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public bool isSplashed
        {
            get
            {
                if (!vessel) return false;
                if (vessel.situation == Vessel.Situations.SPLASHED) return true;
                else
                    return false;
            }
        }
        
        public Vector3 velocity
		{
			get
			{
				if(!vessel) return Vector3.zero;
				return vessel.GetFwdVector();
			}
		}
        
		public Vector3 position
        {
            get
            {
                return vessel.vesselTransform.position;
            }
        }

		private Vessel vessel;

		public Vessel Vessel
		{
			get
			{
				return vessel;
			}
			set
			{
				vessel = value;
			}
		}
        
		void Awake()
		{
			if(!vessel)
			{
				vessel = GetComponent<Vessel>();
			}

			if(!vessel)
			{
				Destroy (this);
				return;
			}

            //destroy this if a target info is already in the system
            IEnumerator otherInfo = vessel.gameObject.GetComponents<OrXTargetInfo>().GetEnumerator();
            while (otherInfo.MoveNext())
            {
                if ((object)otherInfo.Current != this)
                {
                    Destroy(this);
                    return;
                }
            }

            OrXTargetManager.AddTarget(this);

            vessel.OnJustAboutToBeDestroyed += AboutToBeDestroyed;            
        }

		void OnPeaceEnabled()
		{
			//Destroy(this);
		}

		void OnDestroy()
		{
            vessel.OnJustAboutToBeDestroyed -= AboutToBeDestroyed;
        }

		void Update()
		{
			if(!vessel)
			{
				AboutToBeDestroyed();
			}
			else
			{
				if (vessel.vesselType == VesselType.Debris)
				{
					RemoveFromDatabases();
					OrbitalBodyName = OrXHoloCache.OrXCoords.None;
				}
			}
		}
	
		void AboutToBeDestroyed()
		{
			RemoveFromDatabases();
			Destroy(this);
		}

		public void RemoveFromDatabases()
		{
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Moho].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Eve].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Gilly].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Mun].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Minmus].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Duna].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Ike].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Dres].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Jool].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Laythe].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Vall].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Tylo].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Bop].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Pol].Remove(this);
            OrXTargetManager.TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].Remove(this);
        }
    }
}
