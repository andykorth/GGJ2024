

export function $bitsHasIndex(bits: number, index: number): boolean {
	const bit = 1 << index;
	return (bits & bit) === bit;
}