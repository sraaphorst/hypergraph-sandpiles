// HyperedgeTests.cs
// By Sebastian Raaphorst, 2025.

using Hypergraph.Domain;

namespace Hypergraph.Tests.Domain;

public class HyperedgeTests
{
    [Fact]
    public void VertexSetCannotBeNull()
    {
        IEnumerable<Vertex> vertices = null;
        Assert.Throws<ArgumentNullException>(() => new Hyperedge(vertices));    
    }
    
    [Fact]
    public void VertexSetCannotBeEmpty()
    {
        Assert.Throws<ArgumentException>(() => new Hyperedge([]));
    }
    
    [Fact]
    public void VertexSetCannotContainVerticesWithSameId()
    {
        List<Vertex> vertices = [new Vertex(1), new Vertex(2), new Vertex(1), new Vertex(2)];
        var hyperedge = new Hyperedge(vertices);
        Assert.Equal(2, hyperedge.Vertices.Count);
    }

    [Fact]
    public void TwoHyperedgesWithSameVertexIdsAreEqual()
    {
        var hyperedge1 = new Hyperedge([new Vertex(1), new Vertex(2), new Vertex(3)]);
        var hyperedge2 = new Hyperedge([new Vertex(2), new Vertex(3), new Vertex(1)]);
        Assert.Equal(hyperedge1, hyperedge2);
        Assert.True(hyperedge1 == hyperedge2);
    }
}
