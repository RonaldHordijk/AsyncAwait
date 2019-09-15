# Some notes on working with async/await in c#.

## .Net has quite some support for concurrency
- Async/Await
- Task Parallel Library (TPL)
- PLINQ
- DataFlow [System.Threading.Tasks.Dataflow]
- System.Reactive (RX)
- System.Collections.Immutable
- System.Collections.Concurrent

Microsoft has created several higher level tools for 

***

## Blocking code

``` c#
void ButtonSync_Click(object sender, EventArgs e)
{
  var data = GetDataFromTheCloud();
  var result = ProcessData(data);
  DisplayResult(result);
}
```

A winform application where we do a i/o call an cpu itensive call before showing the result.
If one or both of them take a long time, the main thread is busy and ther is no time to handle window messages.
There is handling of user input or repainting of the screen, the application hangs.

***

## Async version

```C#
async void ButtonAsync_Click(object sender, EventArgs e)
{
  var data = await GetDataFromTheCloudAsync();
  var result = await Task.Run(() => ProcessData(data));
  DisplayResult(result);
}
```

Add the async and await keywords.
Make shure the io function is named async
Force new task for the CPU bound action.

Take note the synchronous version is a bit faster.

***

## Async and Exceptions

```C#
async void ButtonException_Click(object sender, EventArgs e)
{
    try
    {
        await ErrorProneFunction();
        await Task.Run(() => ErrorProneFunction2());
    }
    catch (Exception ex)
    {
        LogException(ex);
    }
}

private async Task ErrorProneFunction()
{
    await Task.Delay(30000);
    throw new Exception("ouch");
}

private void ErrorProneFunction2()
{
    Thread.Sleep(3000);
    throw new Exception("ouch");
}
```

Even when the exception comes from another thead it is handled as expected.


***

## Unit tests

```C#
[Test]
public async Task TestAsync()
{
    var result = await DoActionAsync().ConfigureAwait(false);
    Assert.AreEqual(42, result);
}

private async Task<int> DoActionAsync()
{
    await Task.Delay(1000).ConfigureAwait(false);
    return 42;
}
```

To test async functions just add async Task to your tests.


***

## WhenAll and WhenAny

```C#
async Task WaitForAll() 
{
     var t1 = Task.Run(() => LongCall(6000));
     var t2 = Task.Run(() => LongCall(8000));

     await Task.WhenAll(t1, t2); 
} 

private async Task WaitForOne() 
{
     var t1 = Task.Run(() => LongCall(6000));

     var first = await Task.WhenAll(t1, Task.Delay(2000));
}
```

The name describes the action.

***

## Canceling a Task


```C#
private CancellationTokenSource _cts;

private async void ButtonRun_Click(object sender, EventArgs e)
{
    _cts = new CancellationTokenSource();

    try
    {
        await LongCancelableAction(_cts.Token);
    }
    catch (TaskCanceledException)
    {
        DisplayCancledTask();
    }
}

private void ButtonCancel_Click(object sender, EventArgs e)
{
    _cts?.Cancel();
}

private async Task LongCancelableAction(CancellationToken ct)
{
    await Task.Delay(3000, ct);

    ct.ThrowIfCancellationRequested();

    DoMoreWork();
}
```

***

## Generator

```C#
public static IEnumerable<BigInteger> Fibonacci()
{
  BigInteger f1 = 0;
  BigInteger f2 = 1;
  yield return f1;
  yield return f2;

  while (true)
  {
    (f1, f2) = (f2, f1 + f2);
    yield return f2;
  }
}
```

***

## IAsyncEnumerable [.Net 3.0/ C# 8.0 ]

```C#
static async Task Main(string[] args) {
  await foreach (var item in LoadAllAsync())
    ProcessItem(item);
}

static async IAsyncEnumerable<int> LoadAllAsync() {
  int offset = 0;
  int size = 10;
  while (true)
  {
    var items = await LoadSubsetAsync(offset, size);
    if (items is null || items.Count == 0)
      yield break;

    foreach (var item in items)
      yield return item;

    offset += size;
  }
}

static async Task<List<int>> LoadSubsetAsync(int offset, int size) {
  return await Task.FromResult(Enumerable.Range(offset, size).ToList()).ConfigureAwait(false);
}
```
***

## Async all the way down


***

## Async void


***

## SynchronizationContext / ConfigureAwait(False)

***

## Pros and Cons

Pro
- Concise and clear
- Hide complexity
- Exception capturing

Con
- No local changes (Async everywhere)
- Partial hide complexity
- Easy to use wrong

***

## Links

***



