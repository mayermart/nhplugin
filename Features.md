# Navigation and refactoring #
  * References to properties and fields in mapping files are found. That means you can Ctrl-Click (Go To Declaration) on a mapped property in the mapping file to navigate to its declaration.
  * Full rename refactoring support on mapped properties and fields.
  * You can navigate from the declaration to the mapping file by Go To Usage (or Find Usages).
  * NHibernates access attribute is interpreted so that mapped fields are found. Note that internal or private fields are not found because of a limitation in ReSharpers reference searcher. If mapping file and implementation of the mapped class are in different assemblies the mapping assembly should reference the assembly with the class implementaion in order to find the references.

# Analysis support for .hbm.xml files #
  * Undefined mapped properties are marked
  * Undefined types and namespaces are marked
  * Mapping files that are not embedded as resource are marked (the #1 error)
  * Undefined access attribute values are marked (only the predefined values are supported)
  * The severity of the errors can be configured under ReSharper | Options | Code Inspection | Inspection Severity | NHibernate mappings.