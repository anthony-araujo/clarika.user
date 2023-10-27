import { ICity } from "app/entities/city/city.model";
import { ICountry } from "app/entities/country/country.model";

export interface IState {
  id?: number;
  name?: string;
  cities?: ICity[] | null;
  country?: ICountry | null;
}

export class State implements IState {
  constructor(
    public id?: number,
    public name?: string,
    public cities?: ICity[] | null,
    public country?: ICountry | null
  ) {}
}

export function getStateIdentifier(state: IState): number | undefined {
  return state.id;
}
