Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Resources
Imports System.Windows

' General Information about an assembly is controlled through the following
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("Advanced Visual Basic Chess")>
<Assembly: AssemblyDescription("Advanced Visual Basic Chess follows the typical rules of the classic board game. The goal is to capture the King piece of the opposing team. Each team is given the same set of beginning pieces, each type having its own set of rules regarding movement. A more complete summary of the rules for Chess can be found at this URL (https://www.chess.com/learn-how-to-play-chess).  The application itself organizes the pieces, squares, and game board in discrete data types that often utilize each other. The Piece class is the base class used for all pieces with each type having a subclass (Pawn, Bishop, Knight, Rook, Queen, and King). Each piece contains some information about the piece like its movement abilities, position on the board, and color. A class for each square on the board was created to simplify the communication of information about each item on the grid. Many of the possible moves for each piece are stored in an array of linked lists of Squares, each element containing a list of each progressive move that could be made on a given path.")>
<Assembly: AssemblyCompany("Zachary Stehura")>
<Assembly: AssemblyProduct("AdvVBChess")>
<Assembly: AssemblyCopyright("Copyright ©  2021")>
<Assembly: AssemblyTrademark("")>
<Assembly: ComVisible(false)>

'In order to begin building localizable applications, set
'<UICulture>CultureYouAreCodingWith</UICulture> in your .vbproj file
'inside a <PropertyGroup>.  For example, if you are using US english
'in your source files, set the <UICulture> to "en-US".  Then uncomment the
'NeutralResourceLanguage attribute below.  Update the "en-US" in the line
'below to match the UICulture setting in the project file.

'<Assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)>


'The ThemeInfo attribute describes where any theme specific and generic resource dictionaries can be found.
'1st parameter: where theme specific resource dictionaries are located
'(used if a resource is not found in the page,
' or application resource dictionaries)

'2nd parameter: where the generic resource dictionary is located
'(used if a resource is not found in the page,
'app, and any theme specific resource dictionaries)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>



'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("8d661204-4fae-4cd6-a672-c0e8b2262543")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>
