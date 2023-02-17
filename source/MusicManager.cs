using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Celestia {
    public class MusicManager : GameComponent {
        private new Game Game => (Game) base.Game;

        private float SetVolume = 0.1f;
        private float _volume;
        private Song _nextUp;
        private float _elapsedTransitionTime;
        private float _transitionDuration = 2f;
        private bool _transitionComplete = false;

        public MusicManager(Game Game) : base(Game) {}

        public override void Update(GameTime gameTime)
        {
            if (_elapsedTransitionTime >= _transitionDuration) {
                _volume = 1f;
            }
            else {
                _elapsedTransitionTime += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);

                if (_elapsedTransitionTime >= _transitionDuration / 2f && !_transitionComplete) {
                    if (_nextUp != null) {
                        MediaPlayer.Play(_nextUp);
                        _nextUp = null;
                    } else {
                        Stop();
                    }
                    _transitionComplete = true;
                }

                _volume = Volume(_elapsedTransitionTime);
            }

            MediaPlayer.Volume = _volume * SetVolume;
        }

        public void PlayNow(Song song, bool repeat = true) {
            MediaPlayer.IsRepeating = repeat;

            _elapsedTransitionTime = 0f;
            _transitionComplete = false;
            _nextUp = song;
        }

        public void Stop() {
            MediaPlayer.Stop();
        }

        private float Volume(float x) {
            return Math.Abs(x - (_transitionDuration / 2f));
        }
    }
}