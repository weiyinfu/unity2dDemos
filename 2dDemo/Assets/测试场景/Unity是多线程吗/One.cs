using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

/*
 *
 * 测试多个MonoBehavior是同一个UI线程吗？结论是的，虽然多个GameObject处于不同的代码片段里面，但是Unity在运行的时候会把它们放在同一个进程里面。
 */
public class One : MonoBehaviour
{
    public string UserName;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var p = Process.GetCurrentProcess();
        var t = Thread.CurrentThread;
        if (UserName.ToLower() == "one")
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        Debug.Log($"{UserName}:ProcessId={p.Id} ThreadName{t.Name} ThreadId={t.ManagedThreadId}");
    }
}