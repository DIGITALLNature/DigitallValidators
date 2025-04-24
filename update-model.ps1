dgtp profile select DigitallValidators
if ($LASTEXITCODE -ne 0) {
    return;
}

dgtp codegeneration ./src/Digitall.Model --folder Dataverse --config ./src/Digitall.Model/model.json