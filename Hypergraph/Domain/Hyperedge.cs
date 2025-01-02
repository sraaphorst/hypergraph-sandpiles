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

// public class Hyperedge : IEquatable<Hyperedge>
// {
//     private readonly HashSet<Vertex> _vertices = [];
//     public IReadOnlyCollection<Vertex> Vertices => _vertices;
//
//     public bool AddVertex(Vertex vertex)
//     {
//         return vertex == null ? throw new ArgumentNullException(nameof(vertex)) : _vertices.Add(vertex);
//     }
//
//     public bool RemoveVertex(Vertex vertex)
//     {
//         return vertex == null ? throw new ArgumentNullException(nameof(vertex)) : _vertices.Remove(vertex);
//     }
//
//     public bool ContainsVertex(Vertex vertex)
//     {
//         return vertex == null ? throw new ArgumentNullException(nameof(vertex)) : _vertices.Contains(vertex);
//     }
//
//     public int Size => _vertices.Count;
//
//     public bool Equals(Hyperedge? other)
//     {
//         if (ReferenceEquals(this, other))
//             return true;
//         return other is not null && _vertices.SetEquals(other._vertices);
//     }
//     public override bool Equals(object? obj) => Equals(obj as Hyperedge);
//
//     /// <summary>
//     /// Hash code calculation of the Hyperedge. We need to order first, but this is an operation we will
//     /// seldom call, so performance should not be an issue.
//     /// </summary>
//     /// <returns></returns>
//     public override int GetHashCode()
//     {
//         var hash = new HashCode();
//         foreach (var vertex in _vertices)
//             hash.Add(vertex);
//         return hash.ToHashCode();
//     }
//
//     public static bool operator ==(Hyperedge? left, Hyperedge? right)
//     {
//         if (left is null)
//             return right is null;
//         return left.Equals(right);
//     }
//     
//     public static bool operator !=(Hyperedge? left, Hyperedge? right) => !(left == right);
// }
