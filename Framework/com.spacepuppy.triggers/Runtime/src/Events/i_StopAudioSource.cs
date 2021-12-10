﻿#pragma warning disable 0649 // variable declared but not used.

using UnityEngine;

using com.spacepuppy.Tween;

namespace com.spacepuppy.Events
{

    public class i_StopAudioSource : AutoTriggerable
    {

        #region Fields

        [SerializeField]
        [TriggerableTargetObject.Config(typeof(AudioSource))]
        private TriggerableTargetObject _target;

        [SerializeField]
        private SPTimePeriod _fadeOutDur;

        [SerializeField]
        private DisableMode _disableAudioSource;

        #endregion

        #region Properties

        public TriggerableTargetObject Target
        {
            get { return _target; }
        }

        public SPTimePeriod FadeOutDur
        {
            get { return _fadeOutDur; }
            set { _fadeOutDur = value; }
        }

        public DisableMode DisableAudioSource
        {
            get { return _disableAudioSource; }
            set { _disableAudioSource = value; }
        }

        #endregion


        public override bool Trigger(object sender, object arg)
        {
            if (!this.CanTrigger) return false;

            var targ = _target.GetTarget<AudioSource>(arg);
            if (targ == null) return false;
            if (!targ.isPlaying) return false;

            if (_fadeOutDur.Seconds > 0f)
            {
                float cache = targ.volume;
                SPTween.Tween(targ)
                       .To("volume", _fadeOutDur.Seconds, 0f)
                       .Use(_fadeOutDur.TimeSupplier)
                       .OnFinish((s, e) =>
                       {
                           targ.Stop();
                           targ.volume = cache;
                           switch (_disableAudioSource)
                           {
                               case DisableMode.DisableComponent:
                                   targ.enabled = false;
                                   break;
                               case DisableMode.DisableGameObject:
                                   targ.gameObject.SetActive(false);
                                   break;
                           }
                       })
                       .Play(true);
            }
            else
            {
                targ.Stop();
                switch (_disableAudioSource)
                {
                    case DisableMode.DisableComponent:
                        targ.enabled = false;
                        break;
                    case DisableMode.DisableGameObject:
                        targ.gameObject.SetActive(false);
                        break;
                }
            }

            return true;
        }
    }

}
