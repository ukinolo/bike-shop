type Branded<T, Brand extends string> = T & { __brand: Brand; };
const brand = "cities";
const makeCities = <const T extends Record<string, string>>(obj: T) => {
  return Object.fromEntries(
    Object.entries(obj).map(([k, v]) => [
      k,
      v as Branded<typeof v, typeof brand>,
    ])
  ) as {
      [K in keyof T]: Branded<T[K], typeof brand>;
    };
};
export const cities = makeCities({
  NoviSad: "Novi Sad",
  Kragujevac: "Kragujevac",
  Subotica: "Subotica",
});
export type Cities = (typeof cities)[keyof typeof cities];
