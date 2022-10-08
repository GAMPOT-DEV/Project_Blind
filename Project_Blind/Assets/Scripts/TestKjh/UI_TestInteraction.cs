using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;
using UnityEngine.UI;

namespace Blind
{
    public class UI_TestInteraction : UI_WorldSpace
    {
        enum Texts
        {
            InteractionText
        }
        enum Images
        {
            InteractionImage
        }
        public override void Init()
        {
            base.Init();
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));
        }
        protected override void Start()
        {
            base.Start();
            Get<Text>((int)Texts.InteractionText).text = Text;
        }
    }
}

