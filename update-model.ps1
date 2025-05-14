dgtp profile select DigitallValidators
if ($LASTEXITCODE -ne 0) {
    return;
}

dgtp codegeneration ./src/Digitall.Solutions.Validators --folder Model/Dataverse --config ./src/Digitall.Solutions.Validators/model.json