using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BansheeGz.BGDatabase.BGJsonRepoModel;
public class SourceDataSetting : MonoBehaviour
{


    private void Start()
    {

    }

    public T GetPetSkillInfo<T>(int index, Func<DB_petsSkill, T> fieldSelector)
    {
        var entity = DB_petsSkill.GetEntity(index);
        var fieldValue = fieldSelector(entity);
        return fieldValue;
    }


    public T GetPetInfo<T>(int index, Func<DB_petInfo, T> fieldSelector)
    {
        var entity = DB_petInfo.GetEntity(index);
        var fieldValue = fieldSelector(entity);
        return fieldValue;
    }
}
