import { IUserLocation } from "app/entities/user-location/user-location.model";

export interface ILocationType {
  id?: number;
  name?: string;
  userLocations?: IUserLocation[] | null;
}

export class LocationType implements ILocationType {
  constructor(
    public id?: number,
    public name?: string,
    public userLocations?: IUserLocation[] | null
  ) {}
}

export function getLocationTypeIdentifier(
  locationType: ILocationType
): number | undefined {
  return locationType.id;
}
