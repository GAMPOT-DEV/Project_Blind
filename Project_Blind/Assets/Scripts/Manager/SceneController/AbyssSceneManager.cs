using System;
using System.Collections.Generic;
using Blind.Abyss;
using UnityEngine;

namespace Blind
{
    public class AbyssSceneManager : Manager<AbyssSceneManager>
    {
        [SerializeField] private List<Stage> _stageInfo;
        private IEnumerator<Stage> currentStage;
        public int currentStageIndex;
        public PlayerCharacter player;
        public GameObject fadeOut;


        protected override void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
            base.Awake();
            currentStage = _stageInfo.GetEnumerator();
            currentStage.MoveNext();
            currentStage.Current!.Enable();
            currentStageIndex = 1;
            ShowText("Stage1");
        }

        public void MoveNextStage()
        {
            if (currentStage.Current == null) return;
            var prev = currentStage.Current;
            if (currentStage.MoveNext())
            {
                prev.Disable();
                if (currentStage.Current != null)
                    currentStage.Current.Enable();
                currentStageIndex++;
                Debug.Log("현재 스테이지는 : " + currentStageIndex);
                if (currentStageIndex == 6)
                {
                    
                    currentStage.Current.GetComponent<FadeOutExit>().StartFadeOut();
                    return;
                }
                player._moveVector = Vector3.zero;
                ShowText("Stage" + currentStageIndex);
                GameManager.Instance.SetSpawnPoint(currentStage.Current.GetComponentInChildren<Transform>());

            }
        }

        public void ShowText(string stage)
        {
            UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("의문의 목소리"); //가시성을 위해 임시로  stage 추가, 이후 제거하면 됨
            ui.SetScriptTitle((Define.ScriptTitle)Enum.Parse(typeof(Define.ScriptTitle), stage));
        }




    }
}