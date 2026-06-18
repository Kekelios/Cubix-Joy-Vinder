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
    public float probabiliteTir;
    public int nombreTireursParColonne;
    /// <summary>Nom de la scène suivante. Vide si c'est le dernier niveau.</summary>
    public string sceneSuivante;
}
