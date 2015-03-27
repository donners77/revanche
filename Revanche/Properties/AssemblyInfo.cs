using System.Reflection;
using System.Runtime.CompilerServices;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.
[assembly: AssemblyTitle("Revanche")]
[assembly: AssemblyDescription("General-purpose data management software, originally designed for managing characters in fiction stories.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.
[assembly: AssemblyVersion("1.0.*")]
// The following attributes are used to specify the signing key for the assembly, 
// if desired. See the Mono documentation for more information about signing.
//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]
/*
 * Incarnations:
 * I'm starting to forget these. I've done several "character model object" projects in the past, but there's a definite lineage of programs that lead up to this one. 
 * I settled on sqlite as the backend at first. I first tried to implement this system in Tk/Tcl, because it had sqlite integration, but I couldn't wrap my head around the language and got
 * tired of trying to learn it.
 * After this, I transitioned to python, still using an sqlite backend. As usual with python, I had issues with GUI programs - they're difficult to write and there aren't many good binding libraries.
 * Next, I switched to C#, once I found an appropriate sqlite library. I like C# and Gtk# is well supported and developed. I completed a fully working version of the program and began testing it.
 * Quickly, I found that the way I'd structured the program was stupid and not intuitive - it was very difficult to add new character attributes and structures, and I would have to program every single one.
 * Some of the UI generation was automated, but it was still a significant time investment for each behavior change.
 * I began work to refactor the code and make it more flexible, but I had trouble with this mentally because all the data models were already set up in C# and then translated to sqlite via an ORM I wrote myself.
 * Also, I had been using reflection extensively, and things were starting to get pretty tangled within the code.
 * 
 * That brings us to this project. I had a series of eureka moments - firstly, I wanted the data models to be very easy to edit. C# data model changes involve rewriting and compiling the code, and sqlite data
 * models require you to bust open the database and write a lot of parsing/translation code if you don't have native data models. Clearly, neither system is a good choice for defining what the data models are.
 * I realized that what I needed to do was use a very simple language that could be easily edited by hand. YAML was perfect for this. I then realized that what the program code should ultimately do is set up a
 * flexible type system that could be used and extended in these YAML files - this would allow the user to provide all the data needed for new fields in the YAML without having to edit the code to add new
 * features. I had been relying on the built-in type system of C#, but what I really needed to do was make my own type system, in C#, filled with types defined in YAML.
 */