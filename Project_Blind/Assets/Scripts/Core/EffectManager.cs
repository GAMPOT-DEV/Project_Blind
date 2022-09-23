using UnityEngine;

namespace Blind
{
    public class EffectManager : Manager<EffectManager>
    {
        private const string parentPath = "FX/";
        private const string playerPath = "PlayerAttack/";
        public void PlayFx(string path,Vector2 pos)
        {
            var fx = ResourceManager.Instance.Instantiate(parentPath + path);
            fx.transform.position = pos;
        }

        public void PlayFxFacing(string path, Vector2 pos, Facing face)
        {
            var fx = ResourceManager.Instance.Instantiate(parentPath + path);
            fx.transform.position = pos;
        }

        public void PlayPlayerFx(string path, Vector2 pos,Facing face)
        {
            PlayFx(playerPath + path,pos);
        }
    }
}