import {VersionNumber} from '../../types/foundational';
import {h} from 'preact';
import {$div, $label} from '../../util/$tyle';

const version: VersionNumber = {
	VersionName: 'TODO',
	AutoSet: false,
	Year: 'TODO',
	Month: 'TODO',
	Day: 'TODO',
	Hour: 'TODO',
	Minute: 'TODO',
	Version: 'TODO',
	NameAndVersion: 'GGJ2023',
	
};

export function Watermark() {
	// const version = require('Version') as VersionNumber;
	
	return (
		<W_Watermark>
			<L_Version
				text={version.NameAndVersion}
				class={'monospaced'}
			/>
		</W_Watermark>
	);
}

const W_Watermark = $div('Watermark')`
	position: absolute;
	right: 8px;
	bottom: 8px;
`;

const L_Version = $label('L_Version')`
	font-size: 14px;
	-unity-font-style: bold;
	color: rgba(255, 255, 255, 0.2);
	-unity-text-align: middle-center;
	padding: 0;
	margin: 0;
`;
