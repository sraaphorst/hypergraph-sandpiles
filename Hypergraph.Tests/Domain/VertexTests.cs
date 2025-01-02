// VertexTests.cs
// By Sebastian Raaphorst, 2025.

using System.Collections.Immutable;
using Hypergraph.Domain;

namespace Hypergraph.Tests.Domain;

public class VertexTests
{
    [Fact]
    public void VertexEqualToSelf()
    {
        Vertex v = new(0);
        Assert.Equal(v, v);
        Assert.True(v.Equals(v));
        Assert.False(v.Equals(null));
    }
    
    [Fact]
    public void VerticesWithSameIdAreEqual()
    {
        Vertex v1A = new(1);
        Vertex v1B = new(1);
        
        Assert.Equal(v1A, v1B);
        Assert.True(v1A == v1B);
        Assert.False(v1A != v1B);
    }

    [Fact]
    public void VerticesWithDifferentIdAreNotEqual()
    {
        Vertex v1 = new(1);
        Vertex v2 = new(2);
        Assert.NotEqual(v1, v2);
        Assert.True(v1 != v2);
        Assert.False(v1 == v2);
    }
    
    [Fact]
    public void VerticsWithIdsShouldBeCreated()
    {
        ImmutableHashSet<Vertex> vertexList =
        [
            new(1),
            new(2),
            new(3),
        ];
        Assert.True(vertexList.Select(v => v.Id).ToImmutableHashSet().SetEquals([1, 2, 3]));
    }
}
