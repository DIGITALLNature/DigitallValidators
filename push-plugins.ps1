dotnet build ./src/dgt.solutions.Validators.sln --configuration Release

dgtp profile select DigitallValidators
if ($LASTEXITCODE -ne 0) {
    return;
}

$package = Get-ChildItem -Path "./src/dgt.solutions.Validators/bin/Release" -Filter "dgt.solutions.Validators.*.nupkg" -File | Select-Object $_ -First 1

dgtp push $package.FullName --solution dgt_validators