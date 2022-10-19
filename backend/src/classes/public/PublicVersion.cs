using System.Reflection;

namespace awl_raumreservierung;
/// <summary>
/// Bildet Versionsinformationen des Programms ab
/// Ist nicht static, da sie sonst nicht über Endpoints geliefert werden kann
/// </summary>
public class PublicVersion {
	private readonly string executable = Assembly.GetExecutingAssembly().Location;
	/// <summary>
	/// Erstellungsdatum der Executable
	/// </summary>
	/// <returns></returns>
	public DateTime ExecutableDate => File.GetCreationTimeUtc(executable);

	/// <summary>
	/// Änderungsdatum der Executable
	/// </summary>
	/// <returns></returns>
	public DateTime ModifiedDate => File.GetLastAccessTimeUtc(executable);

	/// <summary>
	/// Assembly-Version
	/// </summary>
	/// <returns></returns>
	public string? Version => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
}
