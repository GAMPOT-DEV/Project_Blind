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
        private int currentStageIndex;
        public PlayerCharacter player;
        public GameObject fadeOut;


        protected override void Awake()
        {
            player = ResourceManager.Instance.Instantiate("Player2").GetComponent<PlayerCharacter>();
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
                if (currentStageIndex == 6)
                {
                    fadeOut.GetComponent<FadeOutExit>().enabled = true;
                    return;
                }
                Debug.Log("���� ���������� : " + currentStageIndex);
                player._moveVector = Vector3.zero;
                ShowText("Stage" + currentStageIndex);

            }
        }

        public void ShowText(string stage)
        {
            UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("��ȿ���" + stage); //���ü��� ���� �ӽ÷�  stage �߰�, ���� �����ϸ� ��
            ui.SetScriptTitle((Define.ScriptTitle)Enum.Parse(typeof(Define.ScriptTitle), stage));
        }




    }
}