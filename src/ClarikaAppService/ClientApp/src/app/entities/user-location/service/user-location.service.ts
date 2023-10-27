import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs";

import { isPresent } from "app/core/util/operators";
import { ApplicationConfigService } from "app/core/config/application-config.service";
import { createRequestOption } from "app/core/request/request-util";
import {
  IUserLocation,
  getUserLocationIdentifier,
} from "../user-location.model";

export type EntityResponseType = HttpResponse<IUserLocation>;
export type EntityArrayResponseType = HttpResponse<IUserLocation[]>;

@Injectable({ providedIn: "root" })
export class UserLocationService {
  protected resourceUrl =
    this.applicationConfigService.getEndpointFor("api/user-locations");

  constructor(
    protected http: HttpClient,
    protected applicationConfigService: ApplicationConfigService
  ) {}

  create(userLocation: IUserLocation): Observable<EntityResponseType> {
    return this.http.post<IUserLocation>(this.resourceUrl, userLocation, {
      observe: "response",
    });
  }

  update(userLocation: IUserLocation): Observable<EntityResponseType> {
    return this.http.put<IUserLocation>(
      `${this.resourceUrl}/${
        getUserLocationIdentifier(userLocation) as number
      }`,
      userLocation,
      { observe: "response" }
    );
  }

  partialUpdate(userLocation: IUserLocation): Observable<EntityResponseType> {
    return this.http.patch<IUserLocation>(
      `${this.resourceUrl}/${
        getUserLocationIdentifier(userLocation) as number
      }`,
      userLocation,
      { observe: "response" }
    );
  }

  find(id: number): Observable<EntityResponseType> {
    return this.http.get<IUserLocation>(`${this.resourceUrl}/${id}`, {
      observe: "response",
    });
  }

  query(req?: any): Observable<EntityArrayResponseType> {
    const options = createRequestOption(req);
    return this.http.get<IUserLocation[]>(this.resourceUrl, {
      params: options,
      observe: "response",
    });
  }

  delete(id: number): Observable<HttpResponse<{}>> {
    return this.http.delete(`${this.resourceUrl}/${id}`, {
      observe: "response",
    });
  }

  addUserLocationToCollectionIfMissing(
    userLocationCollection: IUserLocation[],
    ...userLocationsToCheck: (IUserLocation | null | undefined)[]
  ): IUserLocation[] {
    const userLocations: IUserLocation[] =
      userLocationsToCheck.filter(isPresent);
    if (userLocations.length > 0) {
      const userLocationCollectionIdentifiers = userLocationCollection.map(
        (userLocationItem) => getUserLocationIdentifier(userLocationItem)!
      );
      const userLocationsToAdd = userLocations.filter((userLocationItem) => {
        const userLocationIdentifier =
          getUserLocationIdentifier(userLocationItem);
        if (
          userLocationIdentifier == null ||
          userLocationCollectionIdentifiers.includes(userLocationIdentifier)
        ) {
          return false;
        }
        userLocationCollectionIdentifiers.push(userLocationIdentifier);
        return true;
      });
      return [...userLocationsToAdd, ...userLocationCollection];
    }
    return userLocationCollection;
  }
}
