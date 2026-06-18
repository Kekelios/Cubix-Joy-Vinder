using UnityEngine;

[CreateAssetMenu(menuName = "CJV/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int lignesEnnemis;
    public int colonnesEnnemis;
    public float vitesseDeplacement;
    public float distanceDescente;
    public float espacementEnnemis;
    public bool ennemisCanShoot;
    public float intervalTir;
    public int nombreTireursParColonne;
}
