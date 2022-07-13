using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    public partial class KjhTestScene : BaseScene
    {
        List<GameObject> objs = new List<GameObject>();
        protected override void Init()
        {
            base.Init();

            //카메라, 플레이어 등 생성
            CreateCamera();
            CreateEnv();
            GameManager.Instance.Player = CreatePlayer();
            CreateObj();

            /*----------------------
                    TestCode
            ----------------------*/
            #region TestCode

            #region UI Test
            // 설명

            // UI를 크게 3종류로 나눴음
            // 1. SceneUI
            //  - 각 씬마다 존재하는 고유한 UI (메이플스토리의 HP바 같은 느낌)
            // 2. PopupUI
            //  - 알림창처럼 뜨는 Popup UI
            // 3. WorldSpaceUI
            //  - 상호작용 UI같은 카메라에 고정된 UI가 아닌 맵에 있는 UI

            // UI_Base를 상속받아서 UI를 편하게 관리할 수 있음
            // 1. Bind<T> 를 이용해서 enum의 문자열을 읽어와서 오브젝트의 자식들을 찾으면서 이름과 T 형식이 같은 것을 찾아줌
            //  ex)
            //  enum Texts
            //  {
            //      InteractionText
            //  }
            //  Bind<Text>(typeof(Texts)); 
            //  Bind를 호출하면 Texts에 있는 문자열의 이름과 같은 오브젝트를 찾고 거기에 Text가 있으면 찾아서 자동으로 바인딩 해줌
            // 2. Get<T>를 이용해서 바인딩 된 오브젝트를 불러올 수 있음
            //  ex)
            //  Get<Text>((int)Texts.InteractionText).text = "Press Key";

            // 위에서 쓴 코드는 UI_TestInteraction 코드에서 사용하고 있는 코드임

            // UIManager 설명
            // 1. UIManager.Instance.ShowSceneUI<T>
            //  - SceneUI를 띄워주는 함수, T에는 SceneUI를 상속받은 클래스만 넣을 수 있다.
            //  - UIManager의 SceneUI에 저장된다.
            // 2. UIManager.Instance.ShowPopupUI<T>
            //  - PopupUI를 띄워주는 함수, T에는 PopupUI를 상속받은 클래스만 넣을 수 있다.
            // 3. UIManager.Instance.ShowWorldSpaceUI<T>
            //  - WorldSpaceUI를 띄워주는 함수, T에는 WorldSpaceUI를 상속받은 클래스만 넣을 수 있다.
            // 4. UIManager.Instance.ClosePopupUI : 제일 늦게 뜬 PopupUI를 지운다.
            // 5. UIManager.Instance.CloseWorldSpaceUI : 인자로 받은 WorldSpaceUI를 지운다
            // 6. UIManager.Instance.Clear : 모든 UI를 지우고 SceneUI를 Null로 밀어준다.


            // CODE
            // -------------------------------------------------------------------------------------------------------------------
            //_sceneUI = UIManager.Instance.ShowSceneUI<UI_TestSceneUI>(); 
            // -------------------------------------------------------------------------------------------------------------------
            #endregion

            #region ResourceManager, 오브젝트 풀링 Test
            // ResourceManager, PoolManager 설명

            // 1. 프리팹 불러올 때 ResourceManager.Instance.Instantiate 사용
            //  - 프리팹에 Poolable 클래스 컴포넌트를 붙이지 않으면 그냥 생성
            //  - 프리팹에 Poolable 클래스 컴포넌트가 붙어있으면 오브젝트 풀링 적용 
            //    (Pool에서 꺼내서 사용, 만약 처음 생성하면 Pool을 만들어줌)

            // 2. 씬에서 삭제할 때 ResourceManager.Instance.Destroy 사용
            //  - 오브젝트에 Poolable 클래스 컴포넌트가 붙어있지 않으면 그냥 삭제
            //  - 오브젝트에 Poolable 클래스 컴포넌트가 붙어있으면 비활성화 상태로 만들어서 Pool에 보관

            // 3. 프리팹이 아닌 음악 등을 가져올 때 ResourceManager.Instance.Load<> 사용

            // 4. 메모리가 부족해서 Pool을 삭제해야 할 때 : PoolManager.Instance.Clear 사용

            // ResourceManager, PoolManager에서는 위의 4개 함수만 호출하면 됨!!!!!

            // CODE
            // -------------------------------------------------------------------------------------------------------------------
            //for (int i = 0; i < 10; i++)
            //{
            //    objs.Add(ResourceManager.Instance.Instantiate("TestKjh/abc"));
            //}
            //StartCoroutine(CoDestroyObjs(3));
            // -------------------------------------------------------------------------------------------------------------------
            #endregion

            #region SoundManager
            //SoundManager.Instance.Play("TestSound", Define.Sound.Effect);
            //StartCoroutine(CoStopBGM(2));
            #endregion

            #region 다국어 지원
            ConversationScriptStorage.Instance.SetLanguageNum(Define.Language.KOR);
            #endregion

            #endregion
        }
        private void Update()
        {
            HandleUIKeyInput();
        }
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (UIManager.Instance.UINum != 0)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO 메뉴 UI
            }
        }
        IEnumerator CoDestroyObjs(int time)
        {
            yield return new WaitForSeconds(time);
            for (int i = 0; i < 10; i++)
            {
                ResourceManager.Instance.Destroy(objs[i]);
            }
            yield return new WaitForSeconds(time);
            for(int i = 0; i < 5; i++)
            {
                ResourceManager.Instance.Instantiate("TestKjh/abc");
            }
        }
        IEnumerator CoStopBGM(int time)
        {
            yield return new WaitForSeconds(time);
            SoundManager.Instance.Clear();
        }
        public override void Clear()
        {

        }
    }

    public partial class KjhTestScene : BaseScene
    {
        //Init
        UI_TestSceneUI _sceneUI;
        void CreateCamera()
        {
            ResourceManager.Instance.Instantiate("TestKjh/Camera");
        }
        PlayerCharacter CreatePlayer()
        {
            GameObject go = ResourceManager.Instance.Instantiate("TestKjh/Player");
            return go.GetComponent<PlayerCharacter>();
        }
        void CreateEnv()
        {
            ResourceManager.Instance.Instantiate("TestKjh/Ground");
        }
        void CreateObj()
        {
            for(int i = 0; i < 5; i++)
            {
                GameObject go = ResourceManager.Instance.Instantiate("TestKjh/TestObject");
                go.transform.position += Vector3.right * i * 3;
            }
            ResourceManager.Instance.Instantiate("TestKjh/TestNPC");
        }
    }
}
