# Model Design Domain Specyfic Languag Description

## ModelDesign

``` txt
        The root element for the information model. This scheme allows information modellers to defined UA type in a machine readable form. This definition can be used to generate code and documentation. The file is expected to contain a number of types and their instance declarations. Objects which are unique in the address space can also be defined. A validator is available verify consistancy of the model generator and to create suitable values for optional information. Once the design is validated it can be passed to a generator which creates different types of code or documentation.This XSD File contains comments describing the available scheme for defining models.  the comments provide an explination of the scheme, they do not explain the concept that is being model.  It is assumed that the modeller is familur with thiese concepts via the UA specifications.A XML file used for generating a model must start with a  Model definition The below list is a list of the valid constructs where each construct map to a model concept in UA definition using these construct must be assigned to a UA namespace, by the use of the TargetNamespace attribute, they can further be assigned to an XML namespace and have a default Locale assigned.
``` 

## Namespaces

``` txt
This defines the namespaces used in the model.Each namespace listed should also have a namespace prefix defined in the xs:schema element. The order of the namespaces is significant and used to assigned a numeric index to namespaces when they are used in BrowsePaths specified in the ModelDesign.

Defines a single namespace along with identifiers for the namespace.The Name is used to create a progam constant for the URL. The Prefix is the C# namespace which qualifies the generated types. The InternalPrefix is an optional C# namespace which qualifies the generated types used only by the server.The XmlNamespace is The FilePath is

```

### Name

``` txt
 A symbolic name for the namespace that can used as a variable name.
```

### Prefix

``` txt
The .NET namespace used for the classes produced by the generator
```
### InternalPrefix

``` txt
The .NET namespace used for classes that are only used within a server application.
```
### XmlNamespace
``` txt
 The URI for the XML namespace which the data types belong to if it is different from the URI for the model namespace
```

### XmlPrefix
``` txt
The prefix to be used in the XML file for the XML namespace which the data types belong to. Used only XmlNamespace is set.
```

### FilePath
``` txt
The path to the file containing the design file for the namespace.
```
## 


