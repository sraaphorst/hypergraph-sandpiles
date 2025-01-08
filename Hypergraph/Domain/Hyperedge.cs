// Hyperedge.cs
// By Sebastian Raaphorst, 2025.

using System.Collections.Immutable;

namespace Hypergraph.Domain;

/// <summary>
/// A hyperedge over a set of <see cref="Vertex"/>.
/// </summary>
public record Hyperedge(IReadOnlySet<Vertex> Vertices)
{
    public Hyperedge(IEnumerable<Vertex> vertices)
        : this(vertices.ToImmutableHashSet())
    {
        ArgumentNullException.ThrowIfNull(Vertices, nameof(vertices));
        if (Vertices.Count == 0)
            throw new ArgumentException("Hyperedge must contain at least one vertex.", nameof(vertices));
    }

    public virtual bool Equals(Hyperedge? other)
    {
        if (ReferenceEquals(this, other))
            return true;
        if (other is null)
            return false;
        return Vertices.SetEquals(other.Vertices);
    }
    
    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var vertex in Vertices.OrderBy(v => v.Id))
            hash.Add(vertex);
        return hash.ToHashCode();
    }
}
