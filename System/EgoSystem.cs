﻿using System;
using System.Collections.Generic;

public abstract class EgoSystem
{
#if UNITY_EDITOR
    public bool enabled = true;
#endif

    private HashSet<string> _tags;

    public EgoSystem() { }

	public virtual void CreateBundles( EgoComponent egoComponents ) { }

    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    public bool HasTag( string tag )
    {
        return _tags.Contains( tag );
    }

    public void AddTags( params string[] tags )
    {
        foreach( var tag in tags )
        {
            _tags.Add( tag );
        }
    }
}

public class EgoSystem<EC> : EgoSystem
	where EC : EgoConstraint, new()
{
	protected EC constraint;

	public EgoSystem()
	{
		constraint = new EC();
		constraint.SetSystem( this );
		EgoEvents<AddedGameObject>.AddHandler( e => constraint.CreateBundles( e.egoComponent ) );
		EgoEvents<DestroyedGameObject>.AddHandler( e => constraint.RemoveBundles( e.egoComponent ) );
	}

	public override void CreateBundles( EgoComponent egoComponent )
	{
		constraint.CreateBundles( egoComponent );
	}
}
