using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXSounds : MonoBehaviour
    {
        public static OrXSounds instance;

        public AudioSource sound_OrXBoomstick;
        public AudioSource sound_OrXHailToTheKing;
        public AudioSource sound_SpawnOrXHole;
        public AudioSource sound_SpawnOrXHolyHole;
        public AudioSource sound_OrXFatality;
        public AudioSource sound_OrXGroovy;
        public AudioSource sound_OrXFinishHim;
        public AudioSource sound_OrXSpinachChin;
        public AudioSource sound_OrXSheBitch;
        public AudioSource sound_OrXShutTheDoor;
        public AudioSource sound_SpawnOrXRevenge;
        public AudioSource sound_SpawnOrXOrders;
        public AudioSource sound_SpawnOrXNeeds;
        public AudioSource sound_SpawnOrXWhatsThat;
        public AudioSource sound_SpawnOrXThing;
        public AudioSource sound_CallOfTheGreatWhiteNorth;
        public AudioSource sound_BobDougOverHereHesAHoser;

        public AudioSource sound_BackoffManScientist;
        public AudioSource sound_Jonny5Snowblower;
        public AudioSource sound_KickAssBubbleGum;
        public AudioSource sound_LikeToKnowMore;
        public AudioSource sound_MASH4077;
        public AudioSource sound_MissedItByThatMuch;
        public AudioSource sound_myrifle;
        public AudioSource sound_NeedCorporal;
        public AudioSource sound_TarkinFire;
        public AudioSource sound_ShopSmart;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
            GetSounds();
        }
        private void GetSounds()
        {
            Debug.LogWarning("[OrX Log] GETTING SOUNDS .................");

            sound_ShopSmart = gameObject.AddComponent<AudioSource>();
            sound_ShopSmart.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_ShopSmart");

            sound_ShopSmart.loop = false;
            sound_ShopSmart.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_ShopSmart.dopplerLevel = 0f;
            sound_ShopSmart.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_ShopSmart.minDistance = 0.5f;
            sound_ShopSmart.maxDistance = 2f;

            sound_BackoffManScientist = gameObject.AddComponent<AudioSource>();
            sound_BackoffManScientist.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_BackoffManScientist");

            sound_BackoffManScientist.loop = false;
            sound_BackoffManScientist.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_BackoffManScientist.dopplerLevel = 0f;
            sound_BackoffManScientist.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_BackoffManScientist.minDistance = 0.5f;
            sound_BackoffManScientist.maxDistance = 2f;

            sound_Jonny5Snowblower = gameObject.AddComponent<AudioSource>();
            sound_Jonny5Snowblower.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_Jonny5Snowblower");

            sound_Jonny5Snowblower.loop = false;
            sound_Jonny5Snowblower.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_Jonny5Snowblower.dopplerLevel = 0f;
            sound_Jonny5Snowblower.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_Jonny5Snowblower.minDistance = 0.5f;
            sound_Jonny5Snowblower.maxDistance = 2f;

            sound_KickAssBubbleGum = gameObject.AddComponent<AudioSource>();
            sound_KickAssBubbleGum.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_KickAssBubbleGum");

            sound_KickAssBubbleGum.loop = false;
            sound_KickAssBubbleGum.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_KickAssBubbleGum.dopplerLevel = 0f;
            sound_KickAssBubbleGum.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_KickAssBubbleGum.minDistance = 0.5f;
            sound_KickAssBubbleGum.maxDistance = 2f;

            sound_LikeToKnowMore = gameObject.AddComponent<AudioSource>();
            sound_LikeToKnowMore.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_LikeToKnowMore");

            sound_LikeToKnowMore.loop = false;
            sound_LikeToKnowMore.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_LikeToKnowMore.dopplerLevel = 0f;
            sound_LikeToKnowMore.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_LikeToKnowMore.minDistance = 0.5f;
            sound_LikeToKnowMore.maxDistance = 2f;

            sound_MASH4077 = gameObject.AddComponent<AudioSource>();
            sound_MASH4077.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_MASH4077");

            sound_MASH4077.loop = false;
            sound_MASH4077.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_MASH4077.dopplerLevel = 0f;
            sound_MASH4077.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_MASH4077.minDistance = 0.5f;
            sound_MASH4077.maxDistance = 2f;

            sound_MissedItByThatMuch = gameObject.AddComponent<AudioSource>();
            sound_MissedItByThatMuch.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_MissedItByThatMuch");

            sound_MissedItByThatMuch.loop = false;
            sound_MissedItByThatMuch.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_MissedItByThatMuch.dopplerLevel = 0f;
            sound_MissedItByThatMuch.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_MissedItByThatMuch.minDistance = 0.5f;
            sound_MissedItByThatMuch.maxDistance = 2f;

            sound_myrifle = gameObject.AddComponent<AudioSource>();
            sound_myrifle.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_myrifle");

            sound_myrifle.loop = false;
            sound_myrifle.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_myrifle.dopplerLevel = 0f;
            sound_myrifle.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_myrifle.minDistance = 0.5f;
            sound_myrifle.maxDistance = 2f;

            sound_NeedCorporal = gameObject.AddComponent<AudioSource>();
            sound_NeedCorporal.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_NeedCorporal");

            sound_NeedCorporal.loop = false;
            sound_NeedCorporal.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_NeedCorporal.dopplerLevel = 0f;
            sound_NeedCorporal.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_NeedCorporal.minDistance = 0.5f;
            sound_NeedCorporal.maxDistance = 2f;

            sound_TarkinFire = gameObject.AddComponent<AudioSource>();
            sound_TarkinFire.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_TarkinFire");

            sound_TarkinFire.loop = false;
            sound_TarkinFire.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_TarkinFire.dopplerLevel = 0f;
            sound_TarkinFire.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_TarkinFire.minDistance = 0.5f;
            sound_TarkinFire.maxDistance = 2f;





            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Bob and Doug 
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////

            sound_BobDougOverHereHesAHoser = gameObject.AddComponent<AudioSource>();
            sound_BobDougOverHereHesAHoser.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_BobDougOverHereHesAHoser");

            sound_BobDougOverHereHesAHoser.loop = false;
            sound_BobDougOverHereHesAHoser.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_BobDougOverHereHesAHoser.dopplerLevel = 0f;
            sound_BobDougOverHereHesAHoser.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_BobDougOverHereHesAHoser.minDistance = 0.5f;
            sound_BobDougOverHereHesAHoser.maxDistance = 2f;

            sound_CallOfTheGreatWhiteNorth = gameObject.AddComponent<AudioSource>();
            sound_CallOfTheGreatWhiteNorth.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_CallOfTheGreatWhiteNorth");

            sound_CallOfTheGreatWhiteNorth.loop = false;
            sound_CallOfTheGreatWhiteNorth.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_CallOfTheGreatWhiteNorth.dopplerLevel = 0f;
            sound_CallOfTheGreatWhiteNorth.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_CallOfTheGreatWhiteNorth.minDistance = 0.5f;
            sound_CallOfTheGreatWhiteNorth.maxDistance = 2f;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// SPAWN
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////

            sound_SpawnOrXRevenge = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXRevenge.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXRevenge");

            sound_SpawnOrXRevenge.loop = false;
            sound_SpawnOrXRevenge.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_SpawnOrXRevenge.dopplerLevel = 0f;
            sound_SpawnOrXRevenge.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXRevenge.minDistance = 0.5f;
            sound_SpawnOrXRevenge.maxDistance = 2f;


            sound_SpawnOrXOrders = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXOrders.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXOrders");

            sound_SpawnOrXOrders.loop = false;
            sound_SpawnOrXOrders.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_SpawnOrXOrders.dopplerLevel = 0f;
            sound_SpawnOrXOrders.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXOrders.minDistance = 0.5f;
            sound_SpawnOrXOrders.maxDistance = 2f;



            sound_SpawnOrXNeeds = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXNeeds.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXNeeds");

            sound_SpawnOrXNeeds.loop = false;
            sound_SpawnOrXNeeds.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_SpawnOrXNeeds.dopplerLevel = 0f;
            sound_SpawnOrXNeeds.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXNeeds.minDistance = 0.5f;
            sound_SpawnOrXNeeds.maxDistance = 2f;

            sound_SpawnOrXWhatsThat = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXWhatsThat.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXWhatsThat");

            sound_SpawnOrXWhatsThat.loop = false;
            sound_SpawnOrXWhatsThat.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_SpawnOrXWhatsThat.dopplerLevel = 0f;
            sound_SpawnOrXWhatsThat.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXWhatsThat.minDistance = 0.5f;
            sound_SpawnOrXWhatsThat.maxDistance = 2f;




            sound_SpawnOrXThing = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXThing.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXThing");

            sound_SpawnOrXThing.loop = false;
            sound_SpawnOrXThing.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_SpawnOrXThing.dopplerLevel = 0f;
            sound_SpawnOrXThing.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXThing.minDistance = 0.5f;
            sound_SpawnOrXThing.maxDistance = 2f;




            //////////////////////////////////////////////////////////////////////////////////

            sound_OrXShutTheDoor = gameObject.AddComponent<AudioSource>();
            sound_OrXShutTheDoor.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXShutTheDoor");

            sound_OrXShutTheDoor.loop = false;
            sound_OrXShutTheDoor.volume = GameSettings.AMBIENCE_VOLUME;
            sound_OrXShutTheDoor.dopplerLevel = 0f;
            sound_OrXShutTheDoor.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXShutTheDoor.minDistance = 0.5f;
            sound_OrXShutTheDoor.maxDistance = 1f;

            sound_OrXSheBitch = gameObject.AddComponent<AudioSource>();
            sound_OrXSheBitch.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXSheBitch");

            sound_OrXSheBitch.loop = false;
            sound_OrXSheBitch.volume = GameSettings.AMBIENCE_VOLUME;
            sound_OrXSheBitch.dopplerLevel = 0f;
            sound_OrXSheBitch.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXSheBitch.minDistance = 0.5f;
            sound_OrXSheBitch.maxDistance = 1f;

            sound_OrXFinishHim = gameObject.AddComponent<AudioSource>();
            sound_OrXFinishHim.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXFinishHim");

            sound_OrXFinishHim.loop = false;
            sound_OrXFinishHim.volume = GameSettings.AMBIENCE_VOLUME;
            sound_OrXFinishHim.dopplerLevel = 0f;
            sound_OrXFinishHim.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXFinishHim.minDistance = 0.5f;
            sound_OrXFinishHim.maxDistance = 1f;

            sound_OrXBoomstick = gameObject.AddComponent<AudioSource>();
            sound_OrXBoomstick.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXBoomstick");

            sound_OrXBoomstick.loop = false;
            sound_OrXBoomstick.volume = GameSettings.AMBIENCE_VOLUME;
            sound_OrXBoomstick.dopplerLevel = 0f;
            sound_OrXBoomstick.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXBoomstick.minDistance = 0.5f;
            sound_OrXBoomstick.maxDistance = 1f;

            sound_OrXHailToTheKing = gameObject.AddComponent<AudioSource>();
            sound_OrXHailToTheKing.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXHailToTheKing");

            sound_OrXHailToTheKing.loop = false;
            sound_OrXHailToTheKing.volume = GameSettings.AMBIENCE_VOLUME;
            sound_OrXHailToTheKing.dopplerLevel = 0f;
            sound_OrXHailToTheKing.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXHailToTheKing.minDistance = 0.5f;
            sound_OrXHailToTheKing.maxDistance = 1f;


            sound_OrXFatality = gameObject.AddComponent<AudioSource>();
            sound_OrXFatality.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXFatality");

            sound_OrXFatality.loop = false;
            sound_OrXFatality.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_OrXFatality.dopplerLevel = 0f;
            sound_OrXFatality.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXFatality.minDistance = 0.5f;
            sound_OrXFatality.maxDistance = 2f;


            sound_OrXGroovy = gameObject.AddComponent<AudioSource>();
            sound_OrXGroovy.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXGroovy");

            sound_OrXGroovy.loop = false;
            sound_OrXGroovy.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_OrXGroovy.dopplerLevel = 0f;
            sound_OrXGroovy.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXGroovy.minDistance = 0.5f;
            sound_OrXGroovy.maxDistance = 2f;

            sound_OrXSpinachChin = gameObject.AddComponent<AudioSource>();
            sound_OrXSpinachChin.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_OrXSpinachChin");

            sound_OrXSpinachChin.loop = false;
            sound_OrXSpinachChin.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_OrXSpinachChin.dopplerLevel = 0f;
            sound_OrXSpinachChin.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_OrXSpinachChin.minDistance = 0.5f;
            sound_OrXSpinachChin.maxDistance = 2f;


            sound_SpawnOrXHole = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXHole.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXHole");

            sound_SpawnOrXHole.loop = false;
            sound_SpawnOrXHole.volume = GameSettings.AMBIENCE_VOLUME;
            sound_SpawnOrXHole.dopplerLevel = 0f;
            sound_SpawnOrXHole.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXHole.minDistance = 0.5f;
            sound_SpawnOrXHole.maxDistance = 1f;

            sound_SpawnOrXHolyHole = gameObject.AddComponent<AudioSource>();
            sound_SpawnOrXHolyHole.clip = GameDatabase.Instance.GetAudioClip("OrX/Sounds/sound_SpawnOrXHolyHole");

            sound_SpawnOrXHolyHole.loop = false;
            sound_SpawnOrXHolyHole.volume = GameSettings.AMBIENCE_VOLUME * 1.5f;
            sound_SpawnOrXHolyHole.dopplerLevel = 0f;
            sound_SpawnOrXHolyHole.rolloffMode = AudioRolloffMode.Logarithmic;
            sound_SpawnOrXHolyHole.minDistance = 0.5f;
            sound_SpawnOrXHolyHole.maxDistance = 2f;
        }

        public void PlayOrders()
        {
            int random = new System.Random().Next(0, 10);
            if (random <= 5)
            {
                sound_SpawnOrXOrders.Play();
            }
            else
            {
                sound_SpawnOrXThing.Play();
            }
        }
        public void WaldoSound()
        {
            int random = new System.Random().Next(1, 4);

            if (random == 1)
            {
                sound_myrifle.Play();
            }

            if (random == 2)
            {
                sound_BackoffManScientist.Play();
            }

            if (random == 3)
            {
                sound_KickAssBubbleGum.Play();
            }

            if (random == 4)
            {
                sound_Jonny5Snowblower.Play();
            }
        }
        public void TardisSound()
        {
            int random = new System.Random().Next(1, 4);

            if (random == 1)
            {
                sound_NeedCorporal.Play();
            }

            if (random == 2)
            {
                sound_OrXShutTheDoor.Play();
            }

            if (random == 3)
            {
                sound_NeedCorporal.Play();
            }

            if (random == 4)
            {
                sound_OrXShutTheDoor.Play();

            }
        }
        public void ShopSmart()
        {
            int random = new System.Random().Next(0, 10);
            if (random <= 5)
            {
                sound_ShopSmart.Play();
            }
            else
            {
                sound_MASH4077.Play();
            }
        }
        public void KnowMore()
        {
            sound_LikeToKnowMore.Play();
        }
        public void PlayCraftSelected()
        {
            int random = new System.Random().Next(0, 15);
            if (random <= 5)
            {
                sound_OrXHailToTheKing.Play();
            }
            else
            {
                if (random <= 10)
                {
                    sound_SpawnOrXRevenge.Play();
                }
                else
                {
                    sound_NeedCorporal.Play();
                }
            }
        }

        public void BobDougSound()
        {
            StartCoroutine(BobDougSoundRoutine());
        }
        IEnumerator BobDougSoundRoutine()
        {
            int random = new System.Random().Next(1, 4);

            yield return new WaitForSeconds(2);

            if (random == 1)
            {
                sound_CallOfTheGreatWhiteNorth.Play();
            }

            if (random == 2)
            {
                sound_BobDougOverHereHesAHoser.Play();
            }

            if (random == 3)
            {
                sound_TarkinFire.Play();
            }

            if (random == 4)
            {
                sound_OrXFinishHim.Play();
            }

        }


    }
}

