using System.Collections;
using Blind;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// 유닛 체력 클래스를 테스트합니다.
/// </summary>
public class UnitHPTest
{
    /// <summary>
    /// 10으로 초기화 했을 때 제대로 초기화가 되었는지 확인
    /// </summary>
    [Test]
    public void CreateHealth()
    {
        var hp = new UnitHP(10);
        
        Assert.AreEqual(10,hp.GetHP());
    } 
    /// <summary>
    /// 데미지를 제대로 입는지 확인
    /// </summary>
    [Test]
    public void GetDamaged()
    {
        var hp = new UnitHP(10);
        
        Assert.AreEqual(10,hp.GetHP());
        
        hp.GetDamage(5);
        
        Assert.AreEqual(5,hp.GetHP());
    } 
    /// <summary>
    /// 체력이 회복하는지 확인하고 최대 체력 이상으로 회복되지 않는지 확인
    /// </summary>
    
    [Test]
    public void GetHealed()
    {
        var hp = new UnitHP(10);
        
        Assert.AreEqual(10,hp.GetHP());
        hp.GetDamage(5);
        Assert.AreEqual(5,hp.GetHP());
        hp.GetHeal(5);
        Assert.AreEqual(10,hp.GetHP());
        hp.GetHeal(3);
        Assert.AreEqual(10,hp.GetHP());
    } 
}