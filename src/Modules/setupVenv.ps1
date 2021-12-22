# set variables for setup
$ModuleDirectory = $args[0]
$VenvName = "venv"
$ActivateScript = ".\$VenvName\Scripts\Activate.ps1"

# setup python environment
Set-Location -Path $ModuleDirectory
Write-Host "Creating virtual environment for module $ModuleDirectory"
python -m venv $VenvName
& $ActivateScript # .\venv\Scripts\Activate.ps1
Write-Host "Installing requirements for module $ModuleDirectory"
pip install -r requirements.txt
Write-Host "Finished environment setup"
Set-Location $PSScriptRoot
