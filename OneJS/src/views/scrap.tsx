// function TestListStuff(props) {
// 	const gameSys = require('GameSys') as GameSysClip;
//
// 	const testObjList = gameSys.TestObjList;
// 	const testObjArray = gameSys.TestObjArray;
// 	const testStringList = gameSys.TestStringList;
// 	const testStringArray = gameSys.TestStringArray;
//
// 	lg(`testObjList: ${testObjList}, isIterable: ${isIterable(testObjList)}`, this);
// 	lg(`testObjArray: ${testObjArray}, isIterable: ${isIterable(testObjArray)}`, this);
// 	lg(`testStringList: ${testStringList}, isIterable: ${isIterable(testStringList)}`, this);
// 	lg(`testStringArray: ${testStringArray}, isIterable: ${isIterable(testStringArray)}`, this);
//
// 	// testStringArray.push('adding2');
// 	lg(`${typeof testObjList}, ${Object.keys(testObjList).join()}`, testObjList);
// 	lg(`isArray: ${isArrayLike(testObjList)}, ${Array.isArray(testObjList)}`)
// 	lg(`${JSON.stringify(testObjList)}`);
// 	lg(`${JSON.stringify(testStringList)}`);
//
// 	// for (let o of testObjList) {
// 	// 	lg(`o: ${o} ${JSON.stringify(o)}`);
// 	// }
//
// 	lg(`-- ${Object.keys(testObjList).join()} ${Object.keys(testObjList).map(k => testObjList[k])}`);
//
// 	for (let testObj of testObjArray) {
// 		lg(`${testObj} ${Object.keys(testObj).join()} ${Object.keys(testObj).map(k => testObj[k]).join()}`, this)
// 	}
//
//
// 	// const testTrackList = gameSys.TestTrackList;
// 	// lg(`testTrackList: ${testTrackList}, isIterable: ${isIterable(testTrackList)}`, this);
//
// 	const testTrackList = useTrack(gameSys.TestTrackList);
//
//
// 	return (
// 		<FooDiv>
// 			<button onClick={() => gameSys.EvtTest(Math.floor(Math.random() * 100))}/>
//
// 			<div>
// 				<label text={'testObjList'}/>
// 				{$map(testObjList, obj => (
// 					<label text={obj.Name}/>
// 				)) as any}
// 				<label text={'-----------'}/>
// 			</div>
// 			<div>
// 				<label text={'testObjArray'}/>
// 				{testObjArray.map(obj => (
// 					<label text={obj.Name}/>
// 				)) as any}
// 				<label text={'-----------'}/>
// 			</div>
//
// 			<div>
// 				<label text={'testStringArray'}/>
// 				{testStringArray.map(str => (
// 					<label text={str}/>
// 				)) as any}
// 				<label text={'-----------'}/>
// 			</div>
//
// 			<div>
// 				<label text={'testStringList'}/>
// 				{$map(testStringList, str => (
// 					<label text={str}/>
// 				)) as any}
// 				<label text={'-----------'}/>
// 			</div>
//
// 			<div>
// 				<label text={'testTrackList'}/>
// 				{$map(testTrackList, str => (
// 					<label text={str}/>
// 				)) as any}
// 				<label text={'-----------'}/>
// 			</div>
// 		</FooDiv>
// 	);
// }
//
// const FooDiv = sty.div`
//   position: absolute;
//   left: 500px;
//   top: 200px;
// `;
//
// function TestGameSys(props) {
// 	const gameSys = require('GameSys') as GameSysClip;
//
// 	const roomIdf = useTrack(gameSys.RoomIdf);
// 	const status = useTrack(gameSys.Status);
//
// 	const testStr = useTrack(gameSys.TestStr);
// 	const [testInt, setTestInt] = useTrackSet(gameSys.TestInt);
//
//
// 	lg(`render TestGameSys: ${roomIdf} ${status}`, this);
//
// 	const onClick = () => {
// 		lg(`onClick: ${testInt} => ${testInt + 1}`);
// 		setTestInt(testInt + 1);
// 	};
//
// 	return (
// 		<div>
// 			<label text={roomIdf}/>
// 			<label text={status}/>
// 			<label text={testStr}/>
// 			<label text={`${testInt}`}/>
// 			<button
// 				text={'clicky'}
// 				onClick={onClick}
// 			/>
// 		</div>
// 	);
// }
//
//
// function BoxView(props) {
// 	const title = props.title;
//
// 	log(`render BoxView`);
//
// 	return (
// 		<box>
// 			<label text={title}/>
// 		</box>
// 	);
// }
//
// function TempTests() {
// 	return (
// 		<div>
// 		</div>
// 	);
// }
//
// function TempTests2() {
// 	return (
// 		<div
// 			class={emo`
//             margin-top: 100px;
//             margin-left: 300px;
//             width: 600px;
//         `}
// 		>
// 			<BoxView title={`test`}/>
// 			<label text='asdf sdaklf jsdalkf jsdlkfj'/>
// 			<box>
// 				<toggle name='tog a' label='Toggle A' value={true}/>
// 				<toggle
// 					name='tog b'
// 					label='Toggle B'
// 					value={false}
// 				/>
// 				<toggle
// 					name='tog c'
// 					label='Toggle C'
// 				/>
// 			</box>
// 			<radiobuttongroup>
// 				<radiobutton
// 					name='rb a'
// 					label='Test A'
// 					value={true}
// 				/>
// 				<radiobutton
// 					name='rb b'
// 					label='Test B'
// 					value={false}
// 				/>
// 				<radiobutton
// 					name='rb c'
// 					label='Test C'
// 					value={false}
// 				/>
// 			</radiobuttongroup>
// 			<box>
// 				<button
// 					name='cancel'
// 					text='Cancel'
// 					onClick={(e) => {
// 						log('Foo');
// 						(
// 							e.currentTarget as Focusable
// 						).Blur();
// 					}}
// 				/>
// 				<button
// 					name='ok'
// 					text='OK'
// 				/>
// 				<textfield
// 					onInput={(e) => log(e.newData)}
// 					onKeyDown={(e) => log(e.keyCode)}
// 				/>
// 			</box>
// 		</div>
// 	);
// }