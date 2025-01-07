using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class CreateAudioObject : AdvancedMissileBase {
        private GameObject m_audioObj_bgm;
        private AudioSource m_audioSource_bgm;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();

            parent.AudioSourceSEObj = new GameObject("AudioSource_SE");
            parent.AudioSourceSEObj.transform.parent = this.transform;
            parent.AudioSourceSEObj.transform.localPosition = Vector3.zero;
            parent.AudioSourceSEObj.AddComponent<AudioSource>();
            parent.AudioSourceSE = parent.AudioSourceSEObj.GetComponent<AudioSource>();
            parent.AudioSourceSE.spatialBlend = 1;

            m_audioObj_bgm = new GameObject("AudioSource_SE_MOVE");
            m_audioObj_bgm.transform.parent = this.transform;
            m_audioObj_bgm.transform.localPosition = Vector3.zero;
            m_audioObj_bgm.AddComponent<AudioSource>();
            m_audioSource_bgm = m_audioObj_bgm.GetComponent<AudioSource>();
            m_audioSource_bgm.spatialBlend = 1;
            m_audioSource_bgm.loop = true;
            m_audioSource_bgm.clip = parent.MoveSe;
            if (m_audioSource_bgm.clip != null) {
                m_audioSource_bgm.Play();
            }
        }

        void Start() {
            if (parent.ShotSe != null) {
                parent.AudioSourceSE.PlayOneShot(parent.ShotSe, parent.ShotSEVolume);
            }
        }
    }
}