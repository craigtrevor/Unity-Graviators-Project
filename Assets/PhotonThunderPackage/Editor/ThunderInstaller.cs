using UnityEngine;
using UnityEditor;
using System.IO;

namespace Photon.Thunder
{
	[InitializeOnLoad]
	public class ThunderInstaller
	{
		private static string THUNDER_DEFINE = "PHOTON_THUNDER";

		private static string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/PhotonThunderPackage/");
		private static string destinationPath = Path.Combine(EditorApplication.applicationContentsPath, "UnityExtensions/Unity/Networking/");

		private static string engineFileName = "UnityEngine.Networking";
		private static string editorFileName = "UnityEditor.Networking";
		private static string versionFile = "PhotonThunderVersion.txt";

		private static void SetThunderDirective(bool insert)
		{
			string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			
			if (insert) 
			{
				// Define THUNDER
				currentDefines += string.Format(";{0}", THUNDER_DEFINE);
			} else 
			{
				// Undefine THUNDER
                currentDefines = currentDefines.Replace(THUNDER_DEFINE, "");
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefines);
		}

		private static bool HasThunderDirective()
		{
			string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			return currentDefines.Contains(THUNDER_DEFINE);
		}

		static ThunderInstaller() 
		{
			InstallThunder();
		}

		[MenuItem("Window/Thunder/Install")]
		static void InstallThunder()
		{
			string versionFilePath = Path.GetFullPath(Path.Combine(sourcePath, versionFile));
			string versionFilePathDest = Path.GetFullPath(Path.Combine(destinationPath, versionFile));
			int currentVersion = GetVersion(versionFilePath);
			int installedVersion = GetVersion(versionFilePathDest);

			if (!File.Exists(versionFilePathDest) || currentVersion > installedVersion)
			{
				Install(currentVersion, sourcePath, destinationPath, engineFileName, editorFileName, versionFile);
			}
			else
			{
				if (!HasThunderDirective())
				{
					SetThunderDirective(true);
					Debug.Log(string.Format("Installed Thunder Directive in this project", currentVersion));
				}
			}
		}

		[MenuItem("Window/Thunder/Un-Install")]
		static void UnInstallThunder()
		{
			bool restult = EditorUtility.DisplayDialog("Thunder Uninstall", "Unity will be closed after the uninstall process finishess.", "OK", "Cancel");
			if (!restult) { return; }

            try
            {
				// Recover Networking DLLs
                Recover(destinationPath, engineFileName);
                Recover(Path.Combine(destinationPath, "Standalone/"), engineFileName);
                Recover(Path.Combine(destinationPath, "Editor/"), editorFileName);

				// Remove Version File
                FileUtil.DeleteFileOrDirectory(destinationPath + versionFile);

				// Remove Thunder directive
                SetThunderDirective(false);

                EditorUtility.DisplayDialog("Thunder Uninstall", "Unity will now be closed to complete Thunder's uninstall process.", "OK");
                EditorApplication.Exit(0);
            }
            catch(IOException) 
            {
                EditorUtility.DisplayDialog("Thunder Uninstall Warning", "Remove file permission failure. Please open Unity Editor as Administrator and retry.", "OK");
            }
		}

		private static void Install(int currentVersion, string sourcePath, string destinationPath, string engineFileName, string editorFileName, string versionFile)
		{
			bool restult = EditorUtility.DisplayDialog("Thunder Install", "Unity will be closed after the installation finishess.", "OK", "Cancel");
			if (!restult) { return; }

			Debug.Log(string.Format("Installing version v{0}", currentVersion));

			string srcVersionPath = Path.GetFullPath(Path.Combine(sourcePath, versionFile));
			string desVersionPath = Path.GetFullPath(Path.Combine(destinationPath, versionFile));

            try
            {	
				// Copy or replace version file
				File.Copy(srcVersionPath, desVersionPath, true);

                BackupAndReplace(Path.GetFullPath(Path.Combine(sourcePath, "Networking/")), destinationPath, engineFileName);
                BackupAndReplace(Path.GetFullPath(Path.Combine(sourcePath, "Networking/Standalone/")), Path.GetFullPath(Path.Combine(destinationPath, "Standalone/")), engineFileName);
                BackupAndReplace(Path.GetFullPath(Path.Combine(sourcePath, "Networking/Editor/")), Path.GetFullPath(Path.Combine(destinationPath, "Editor/")), editorFileName);

				SetThunderDirective(true);

                Debug.LogWarning("Please restart the Unity Editor");

                EditorUtility.DisplayDialog("Thunder Install", "Unity will now be closed to complete the installation.", "OK");
                EditorApplication.Exit(0);
            }
            catch(IOException)
            {
                EditorUtility.DisplayDialog("Thunder Install Warning", "Copy file permission failure. Please open Unity Editor as Administrator and retry.", "OK");
            }
		}

		private static void BackupAndReplace(string sourcePath, string destPath, string fileName)
		{

			string sourceFile = Path.GetFullPath(Path.Combine(sourcePath, fileName));
			string destFile = Path.GetFullPath(Path.Combine(destPath, fileName));

			// Backup original UNet file, both dll and debug figures (don't do twice)
			if (!File.Exists(destFile + ".dll.bckp"))
			{
				FileUtil.CopyFileOrDirectory(destFile + ".dll", destFile + ".dll.bckp");
				FileUtil.CopyFileOrDirectory(destFile + ".dll.mdb", destFile + ".dll.mdb.bckp");

				Debug.Log(string.Format("Backup done: {0}", fileName));
			}
			else
			{
				Debug.Log(string.Format("Skip backup: {0}", fileName));
			}

			// replace with Thunder's
			FileUtil.ReplaceFile(sourceFile + ".dll.thdr", destFile + ".dll");
			FileUtil.ReplaceFile(sourceFile + ".dll.mdb.thdr", destFile + ".dll.mdb");
		}

		private static void Recover(string destPath, string fileName)
		{
			string destFile = Path.GetFullPath(Path.Combine(destPath, fileName));

			if (File.Exists(destFile + ".dll.bckp"))
			{
				FileUtil.ReplaceFile(destFile + ".dll.bckp", destFile + ".dll");
				FileUtil.ReplaceFile(destFile + ".dll.mdb.bckp", destFile + ".dll.mdb");

				FileUtil.DeleteFileOrDirectory(destFile + ".dll.bckp");
				FileUtil.DeleteFileOrDirectory(destFile + ".dll.mdb.bckp");

				Debug.Log(string.Format("Recover done: {0}", fileName));
			}
		}

		private static int GetVersion(string path)
		{
			int version = -1;

			if (File.Exists(path))
			{
				int.TryParse(File.ReadAllText(path), out version);
			}

			return version;
		}
	}
}
