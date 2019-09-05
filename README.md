# Some notes on working with async/await in c#.

## .Net has quite some support for concurrency
- Async/Await
- Task Parallel Library (TPL)
- PLINQ
- DataFlow [System.Threading.Tasks.Dataflow]
- System.Reactive (RX)
- System.Collections.Immutable
- System.Collections.Concurrent

***

## Blocking code

``` c#
private void ButtonSync_Click(object sender, EventArgs e)
{
  var data = GetDataFromTheCloud();
  var result = ProcessData(data);
  DisplayResult(result);
}
```

***

## Async version

```C#
private async void ButtonAsync_Click(object sender, EventArgs e)
{
  var data = await GetDataFromTheCloudAsync();
  var result = await Task.Run(() => ProcessData(data));
  DisplayResult(result);
}
```

Add the async and await keywords.
Make shure the io function is named async
Force new task for the CPU bound action.

Take note the synchronouse version was a bet faster.

***

## Async and Exceptions

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

***

## Generator
``` c#
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

## IAsyncEnumerable [.Net 3.0/ C# 8.0
``` c#
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

***s

## Links



