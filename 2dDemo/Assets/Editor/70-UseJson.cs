using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

/*
 * 1. 不要使用Unity自带的JsonUtility，它有很多坑，不要在它上面浪费使用
 * 2. JsonUtility无法序列化数组
 * 3. JsonUtility无法把对象的public readonly字段序列化
 */
[TestFixture]
class UseJson
{
    class User
    {
        private string name = "weiyinfu";
        public int age = 13;
        public readonly string address = "beijing";
    }


    [Test]
    public void TestJson()
    {
        var usr = new User();
        var s = JsonUtility.ToJson(usr, true);
        Debug.Log($"usr {s}");
        Debug.Log($"User={JsonUtility.ToJson(usr)}");
        MemberInfo info = typeof(User);
        foreach (var i in info.CustomAttributes)
        {
            Debug.Log($"{i}");
        }

        var t = usr.GetType();
        Debug.Log(t.GetFields().Length);
        foreach (var i in t.GetFields())
        {
            var ans = i.GetValue(usr);
            Debug.Log($"{i.Name}=>{ans}");
        }

        /*
         * 使用runtimeFields能够打印私有成员变量
         */
        Debug.Log("runtime fields");
        foreach (var i in t.GetRuntimeFields())
        {
            var ans = i.GetValue(usr);
            Debug.Log($"{i.Name}=>{ans}");
        }
    }

    [Test]
    public void testReflectStaticField()
    {
        //测试通过反射修改静态字段

        var t = typeof(TouchScreenKeyboard);
        foreach (var i in t.GetProperties())
        {
            Debug.Log($"property:{i.Name} {i.CanRead} {i.CanWrite}");
        }

        Debug.Log($"members=========");
        foreach (var i in t.GetMembers())
        {
            Debug.Log($"name={i.Name}");
        }

        // Debug.Log(TouchScreenKeyboard.isInPlaceEditingAllowed);

        Debug.Log("+===========RuntimeProperties====**************");
        foreach (var i in t.GetRuntimeProperties())
        {
            Debug.Log($"{i.Name}=>{i.GetValue(TouchScreenKeyboard.Open(""))}");
            if (i.Name.Equals("disableInPlaceEditing"))
            {
                Debug.Log("==========");
                Debug.Log(i.GetValue(TouchScreenKeyboard.Open("")));
                i.SetValue(TouchScreenKeyboard.Open(""), !TouchScreenKeyboard.isInPlaceEditingAllowed);
                Debug.Log(i.GetValue(TouchScreenKeyboard.Open("")));
            }
        }

        // var x = t.GetRuntimeProperty("disableInPlaceEditing");
        // Debug.Log($"x={x}");
        // x.SetValue(TouchScreenKeyboard.Open(""), false);
        // Debug.Log(TouchScreenKeyboard.isInPlaceEditingAllowed);
    }

    [Test]
    public void testJsonArray()
    {
        User u = new User();
        User[] a = new User[] {u, u, u};
        var ans = JsonUtility.ToJson(a);
        Debug.Log($"{ans}"); //输出{}，这说明Unity自带的JSON是一个不可用的JSON
        ans = JsonConvert.SerializeObject(a); //这个是好用的
        Debug.Log(ans);
        var x = JsonConvert.DeserializeObject(ans);
        Debug.Log(x.ToString());
        var b = JsonConvert.DeserializeObject<User[]>(ans);
        Debug.Log(b.Length);
    }
}