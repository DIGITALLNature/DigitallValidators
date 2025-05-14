dotnet build ./src/Digitall.Solutions.Validators.slnx --configuration Release

dgtp profile select DigitallValidators
if ($LASTEXITCODE -ne 0) {
    return;
}

$package = Get-ChildItem -Path "./src/Digitall.Solutions.Validators/bin/Release" -Filter "Digitall.Solutions.Validators.*.nupkg" -File | Select-Object $_ -First 1

dgtp push $package.FullName --solution dgt_validators