﻿using System;
using System.Collections.Generic;

using UML=TSF.UmlToolingFramework.UML;

namespace TSF.UmlToolingFramework.Wrappers.EA {
  public abstract class Element : UML.Classes.Kernel.Element {
    internal Model model { get;  set; }

    internal Element(Model model){
      this.model = model;
    }

    public abstract String notes { get; set; }
    public abstract HashSet<UML.Classes.Kernel.Element> ownedElements 
      { get; set; }
    public abstract HashSet<UML.Classes.Kernel.Comment> ownedComments 
      { get; set; }
    public abstract UML.Classes.Kernel.Element owner 
      { get; set; }
    public abstract HashSet<UML.Profiles.Stereotype> stereotypes 
      { get; set; }
    public void addStereotype(UML.Profiles.Stereotype stereotype) {
      HashSet<UML.Profiles.Stereotype> newStereotypes = 
        new HashSet<UML.Profiles.Stereotype>(this.stereotypes);
      if (!newStereotypes.Contains(stereotype)) {
        newStereotypes.Add(stereotype);
        this.stereotypes = newStereotypes;
      }
    }

    /// returns the owner of the given type
    /// This operation will keep on looking upwards through the owners until 
    /// it finds one with the given type.
    /// NON UML
    public T getOwner<T>() where T : UML.Classes.Kernel.Element {
      if (this.owner is T || this.owner == null) {
        return (T) this.owner;
      }else {
        return ((Element)this.owner).getOwner<T>();
      }
    }

    /// default implementation returns an empty list because there is only one
    /// subclass that can actually implement this operation: ElementWrapper.
    public virtual List<UML.Classes.Kernel.Relationship> relationships {
      get { return new List<UML.Classes.Kernel.Relationship>(); }
      set { /* do nothing */ }
    }

    /// default implementation returns an empty list because there is only one
    /// subclass that can actually implement this operation: EAElementWrapper.
    public virtual List<T> getRelationships<T>() 
      where T : UML.Classes.Kernel.Relationship 
    {
      return new List<T>();
    }


    internal abstract void saveElement();

    public void save(){
      this.saveElement();
      foreach (UML.Classes.Kernel.Element element in this.ownedElements) {
        ((Element)element).save();
      }
    }



    //default not implemented
    public virtual HashSet<T> getUsingDiagrams<T>() where T : class, UML.Diagrams.Diagram
    {
        throw new NotImplementedException();
    }
  	
    /// <summary>
    /// selects the element. 
    /// </summary>
	public virtual void select()
	{
		this.model.selectedElement = this;
	}
  	/// <summary>
  	/// opens the element. 
  	/// </summary>
	public virtual void open()
	{
		this.model.selectedElement = this;
	}

  	
	public abstract TSF.UmlToolingFramework.UML.UMLItem getItemFromRelativePath(List<string> relativePath);
	
	public string name 
	{
		get 
		{
			return string.Empty;
		}
	}
  	/// <summary>
  	/// default empty implementation
  	/// </summary>
	public virtual HashSet<UML.Profiles.TaggedValue> taggedValues {
		get {
			return new HashSet<UML.Profiles.TaggedValue>();
		}
		set {
			//do nothing
		}
	}
  	/// <summary>
  	/// default empty implementation
  	/// </summary>
  	/// <returns>empty set</returns>
	public virtual HashSet<UML.Profiles.TaggedValue> getReferencingTaggedValues()
	{
		return new HashSet<UML.Profiles.TaggedValue>();
	}
  }
}
