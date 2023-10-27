import { IState } from "app/entities/state/state.model";

export interface ICity {
  id?: number;
  name?: string;
  state?: IState | null;
}

export class City implements ICity {
  constructor(
    public id?: number,
    public name?: string,
    public state?: IState | null
  ) {}
}

export function getCityIdentifier(city: ICity): number | undefined {
  return city.id;
}
