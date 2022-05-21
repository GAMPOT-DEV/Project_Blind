using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    public partial class KjhTestScene : BaseScene
    {
        protected override void Init()
        {
            base.Init();

            CreateCamera();
            CreateEnv();
            GameManager.Instance.Player = CreatePlayer();
            CreateObj();
        }
        public override void Clear()
        {

        }
    }

    public partial class KjhTestScene : BaseScene
    {
        //Init
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
            
        }
    }
}
