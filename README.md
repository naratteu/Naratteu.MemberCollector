# Naratteu.MemberCollector
선언적으로 맴버배열을 생성하게 만드는 소스제너레이터 

소스제너레이터와 코드디자인에 대한 실험적 라이브러리 구상 입니다

## usage

``` csharp
partial class Example
{
  [MemberCollect] int a;
  [MemberCollect] int b { get; set; }
  [MemberCollect] int c => 10;
  // generated: System.Collections.IEnumerable GetMemberCollection() => [ a, b, c ];
}
```

# todo
- Generate Generic Collection
- Custom Option
- Disposable Collection
- Support `record`, `struct` etc..