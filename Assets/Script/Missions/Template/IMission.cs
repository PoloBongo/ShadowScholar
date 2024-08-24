using UnityEngine;

public abstract class IMission
{
    public int number;
    public string name;
    public string localization_path_texture;
    public Texture localization_texture;
    public string objectif;

    public IMission(int _number, string _name, string _objectif, string _localization_path_texture)
    {
        number = _number;
        name = _name;
        objectif = _objectif;
        localization_path_texture = _localization_path_texture; 
        localization_texture = Resources.Load<Texture>(_localization_path_texture);
    }
}
