$Cutscene::ModuleDependency = true;

//FUNCTIONS
function eulerToAxis(%euler) //Trader
{
	%euler = VectorScale(%euler,$pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	return getWords(%matrix,3,6);
}

function axisToEuler(%axis) //Trader
{
	%angleOver2 = getWord(%axis,3) * 0.5;
	%angleOver2 = -%angleOver2;
	%sinThetaOver2 = mSin(%angleOver2);
	%cosThetaOver2 = mCos(%angleOver2);
	%q0 = %cosThetaOver2;
	%q1 = getWord(%axis,0) * %sinThetaOver2;
	%q2 = getWord(%axis,1) * %sinThetaOver2;
	%q3 = getWord(%axis,2) * %sinThetaOver2;
	%q0q0 = %q0 * %q0;
	%q1q2 = %q1 * %q2;
	%q0q3 = %q0 * %q3;
	%q1q3 = %q1 * %q3;
	%q0q2 = %q0 * %q2;
	%q2q2 = %q2 * %q2;
	%q2q3 = %q2 * %q3;
	%q0q1 = %q0 * %q1;
	%q3q3 = %q3 * %q3;
	%m13 = 2.0 * (%q1q3 - %q0q2);
	%m21 = 2.0 * (%q1q2 - %q0q3);
	%m22 = 2.0 * %q0q0 - 1.0 + 2.0 * %q2q2;
	%m23 = 2.0 * (%q2q3 + %q0q1);
	%m33 = 2.0 * %q0q0 - 1.0 + 2.0 * %q3q3;
	return mRadToDeg(mAsin(%m23)) SPC mRadToDeg(mAtan(-%m13, %m33)) SPC mRadToDeg(mAtan(-%m21, %m22));
}

function addKeyBind(%div,%name,%cmd,%device,%action,%overWrite) //Greek2Me
{
	if(%device !$= "" && %action !$= "")
	{
		if(moveMap.getCommand(%device,%action) $= "" || %overWrite)
			moveMap.bind(%device,%action,%cmd);
	}

	%divIndex = -1;
	for(%i=0; %i < $RemapCount; %i++)
	{
		if($RemapDivision[%i] $= %div)
		{
			%divIndex = %i;
			break;
		}
	}

	if(%divIndex >= 0)
	{
		for(%i=$RemapCount-1; %i > %divIndex; %i--)
		{
			$RemapDivision[%i+1] = $RemapDivision[%i];
			$RemapName[%i+1] = $RemapName[%i];
			$RemapCmd[%i+1] = $RemapCmd[%i];
		}

		$RemapDivision[%divIndex+1] = "";
		$RemapName[%divIndex+1] = %name;
		$RemapCmd[%divIndex+1] = %cmd;
		$RemapCount ++;
	}
	else
	{
		$RemapDivision[$RemapCount] = %div;
		$RemapName[$RemapCount] = %name;
		$RemapCmd[$RemapCount] = %cmd;
		$RemapCount ++;
	}
}

function searchFields(%string, %searchString)
{
	%fields = getFieldCount(%string);
	for(%i = 0; %i < %fields; %i++)
	{
		%field = getField(%string, %i);
		if(%field $= %searchString)
			return %i;
	}
	return -1;
}

function addLeadingZeros(%src, %num)
{
	%len = strLen(%src);
	%zeros = %num - %len;
	if(%zeros < 0)
		return %src;
	for(%i = 1; %i <= %zeros; %i++)
		%add = %add @ "0";
	return %add @ %src;
}

function formatMSTime(%ms)
{
	%ms = mAbs(%ms);

	%mil = %ms % 1000;
	%sc = mFloor(%ms / 1000);
	%sec = %sc % 60;
	%min = mFloor(%sc / 60);

	return %min @ ":" @ addLeadingZeros(%sec, 2) @ ":" @ addLeadingZeros(%mil, 3);
}

function mRoundToN(%x, %n)
{
	return mFloor((%x / %n) + 0.5) * %n;
}

function bubbleSort(%list)
{
	%ct = getFieldCount(%list);
	for(%i = 0; %i < %ct; %i++)
	{
		for(%k = 0; %k < (%ct - %i - 1); %k++)
		{
			if(getField(%list, %k) > getField(%list, %k+1))
			{
				%tmp = getField(%list, %k);
				%list = setField(%list, %k, getField(%list, %k+1));
				%list = setField(%list, %k+1, %tmp);
			}
		}
	}
	return %list;
}