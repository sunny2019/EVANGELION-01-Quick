using System;
using System.Collections.Generic;

public enum OldPeopleType
{
    男性健康老人,
    女性健康老人,
    男性残疾老人,
    女性残疾老人,
}

public enum Gender
{
    男,
    女,
}

public enum LiveType
{
    独居,
    非独居,
}

public enum HouseType
{
    高层电梯房,
    多层楼梯房,
    新式里弄,
    老式公房等,
}

public enum PropertyStatus
{
    无,
    门禁,
    值班,
}

public enum HousePayStatus
{
    未开通,
    已开通已缴费,
    已开通欠费中,
}

public enum BloodType
{
    A型,
    B型,
    AB型,
    O型,
}

public enum HealthStatus
{
    自理,
    失能,
    半失能,
    失智,
    半失智,
}

public enum LiveStatus
{
    居家,
    不在家,
}

public class OldPeopleData
{
    public string modelName;

    //基本信息
    public string 姓名 = "张三";
    public Gender 性别 = Gender.男;
    public int 年龄 = 25;
    public string 身份证号 = "123456789123456789";
    public string 老人联系方式 = "12345678910";
    public string 紧急联络人联系信息 = "李四 12345678911";
    public string 约定联络人联系信息 = "王五 12345678912";

    //居住信息
    public string 居住地址 = "上海市青浦区新丹路358号4幢101室";
    public LiveType 居住状态 = LiveType.独居;
    public HouseType 住宅种类 = HouseType.新式里弄;
    public PropertyStatus 物业管理情况 = PropertyStatus.门禁;
    public HousePayStatus 水电媒情况 = HousePayStatus.已开通已缴费;

    //医疗健康信息
    public BloodType 血型 = BloodType.A型;
    public HealthStatus 健康状况 = HealthStatus.自理;

    public HealthStatus 残疾情况
    {
        get => 健康状况;
        set => 健康状况 = value;
    }

    public string 既往病史 = "无";
    public string 现病史 = "无";
    public string 手术史 = "无";
    public string 过敏史 = "无";
    public string 出院小结 = "无";
    public string 门诊记录 = "无";
    public string 用药信息 = "无";

    //智能设备监测信息
    public string 实时生理健康信息 = "健康";
    public string 实时心理健康信息 = "健康";
    public LiveStatus 实时居家情况信息 = LiveStatus.居家;
    public string 实时安全情况 = "安全";
    public string 实时呼叫信息 = "李四 12345678911";

    //社交关系信息
    public string 家属信息 = "李四 12345678911";
    public string 邻居信息 = "赵六 12345678913";
    public string 家庭医生信息 = "孙七 12345678913";

    public List<KeyValuePair<string, List<KeyValuePair<string, string>>>> GetOldPeopleData()
    {
        List<KeyValuePair<string, List<KeyValuePair<string, string>>>> dataInfo = new List<KeyValuePair<string, List<KeyValuePair<string, string>>>>();
        List<KeyValuePair<string, string>> 基本信息 = new List<KeyValuePair<string, string>>();
        基本信息.Add(new KeyValuePair<string, string>("姓名", 姓名));
        基本信息.Add(new KeyValuePair<string, string>("性别", 性别.ToString()));
        基本信息.Add(new KeyValuePair<string, string>("年龄", 年龄.ToString()));
        基本信息.Add(new KeyValuePair<string, string>("身份证号", 身份证号));
        基本信息.Add(new KeyValuePair<string, string>("老人联系方式", 老人联系方式));
        基本信息.Add(new KeyValuePair<string, string>("紧急联络人联系信息", 紧急联络人联系信息));
        基本信息.Add(new KeyValuePair<string, string>("约定联络人联系信息", 约定联络人联系信息));
        dataInfo.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>("基本信息", 基本信息));

        List<KeyValuePair<string, string>> 居住信息 = new List<KeyValuePair<string, string>>();
        居住信息.Add(new KeyValuePair<string, string>("居住地址", 居住地址));
        居住信息.Add(new KeyValuePair<string, string>("居住状态", 居住状态.ToString()));
        居住信息.Add(new KeyValuePair<string, string>("住宅种类", 住宅种类.ToString()));
        居住信息.Add(new KeyValuePair<string, string>("物业管理情况", 物业管理情况.ToString()));
        居住信息.Add(new KeyValuePair<string, string>("水电媒情况", 水电媒情况.ToString()));
        dataInfo.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>("居住信息", 居住信息));


        List<KeyValuePair<string, string>> 医疗健康信息 = new List<KeyValuePair<string, string>>();
        医疗健康信息.Add(new KeyValuePair<string, string>("血型", 血型.ToString()));
        医疗健康信息.Add(new KeyValuePair<string, string>("健康状况", 健康状况.ToString()));
        医疗健康信息.Add(new KeyValuePair<string, string>("残疾情况", 残疾情况.ToString()));
        医疗健康信息.Add(new KeyValuePair<string, string>("既往病史", 既往病史));
        医疗健康信息.Add(new KeyValuePair<string, string>("现病史 ", 现病史));
        医疗健康信息.Add(new KeyValuePair<string, string>("手术史 ", 手术史));
        医疗健康信息.Add(new KeyValuePair<string, string>("过敏史 ", 过敏史));
        医疗健康信息.Add(new KeyValuePair<string, string>("出院小结", 出院小结));
        医疗健康信息.Add(new KeyValuePair<string, string>("门诊记录", 门诊记录));
        医疗健康信息.Add(new KeyValuePair<string, string>("用药信息", 用药信息));
        dataInfo.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>("医疗健康信息", 医疗健康信息));

        List<KeyValuePair<string, string>> 智能设备监测信息 = new List<KeyValuePair<string, string>>();
        智能设备监测信息.Add(new KeyValuePair<string, string>("实时生理健康信息", 实时生理健康信息));
        智能设备监测信息.Add(new KeyValuePair<string, string>("实时心理健康信息", 实时心理健康信息));
        智能设备监测信息.Add(new KeyValuePair<string, string>("实时居家情况信息", 实时居家情况信息.ToString()));
        智能设备监测信息.Add(new KeyValuePair<string, string>("实时安全情况", 实时安全情况));
        智能设备监测信息.Add(new KeyValuePair<string, string>("实时呼叫信息", 实时呼叫信息));
        dataInfo.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>("智能设备监测信息", 智能设备监测信息));


        List<KeyValuePair<string, string>> 社交关系信息 = new List<KeyValuePair<string, string>>();
        社交关系信息.Add(new KeyValuePair<string, string>("家属信息", 家属信息));
        社交关系信息.Add(new KeyValuePair<string, string>("邻居信息", 邻居信息));
        社交关系信息.Add(new KeyValuePair<string, string>("家庭医生信息", 家庭医生信息));
        dataInfo.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>("社交关系信息", 社交关系信息));

        return dataInfo;
    }
}