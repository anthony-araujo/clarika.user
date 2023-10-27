import { TestBed } from "@angular/core/testing";
import { HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import {
  ActivatedRouteSnapshot,
  ActivatedRoute,
  Router,
  convertToParamMap,
} from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { of } from "rxjs";

import { IUserLocation, UserLocation } from "../user-location.model";
import { UserLocationService } from "../service/user-location.service";

import { UserLocationRoutingResolveService } from "./user-location-routing-resolve.service";

describe("UserLocation routing resolve service", () => {
  let mockRouter: Router;
  let mockActivatedRouteSnapshot: ActivatedRouteSnapshot;
  let routingResolveService: UserLocationRoutingResolveService;
  let service: UserLocationService;
  let resultUserLocation: IUserLocation | undefined;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([])],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: convertToParamMap({}),
            },
          },
        },
      ],
    });
    mockRouter = TestBed.inject(Router);
    jest
      .spyOn(mockRouter, "navigate")
      .mockImplementation(() => Promise.resolve(true));
    mockActivatedRouteSnapshot = TestBed.inject(ActivatedRoute).snapshot;
    routingResolveService = TestBed.inject(UserLocationRoutingResolveService);
    service = TestBed.inject(UserLocationService);
    resultUserLocation = undefined;
  });

  describe("resolve", () => {
    it("should return IUserLocation returned by find", () => {
      // GIVEN
      service.find = jest.fn((id) => of(new HttpResponse({ body: { id } })));
      mockActivatedRouteSnapshot.params = { id: 123 };

      // WHEN
      routingResolveService
        .resolve(mockActivatedRouteSnapshot)
        .subscribe((result) => {
          resultUserLocation = result;
        });

      // THEN
      expect(service.find).toBeCalledWith(123);
      expect(resultUserLocation).toEqual({ id: 123 });
    });

    it("should return new IUserLocation if id is not provided", () => {
      // GIVEN
      service.find = jest.fn();
      mockActivatedRouteSnapshot.params = {};

      // WHEN
      routingResolveService
        .resolve(mockActivatedRouteSnapshot)
        .subscribe((result) => {
          resultUserLocation = result;
        });

      // THEN
      expect(service.find).not.toBeCalled();
      expect(resultUserLocation).toEqual(new UserLocation());
    });

    it("should route to 404 page if data not found in server", () => {
      // GIVEN
      jest
        .spyOn(service, "find")
        .mockReturnValue(
          of(new HttpResponse({ body: null as unknown as UserLocation }))
        );
      mockActivatedRouteSnapshot.params = { id: 123 };

      // WHEN
      routingResolveService
        .resolve(mockActivatedRouteSnapshot)
        .subscribe((result) => {
          resultUserLocation = result;
        });

      // THEN
      expect(service.find).toBeCalledWith(123);
      expect(resultUserLocation).toEqual(undefined);
      expect(mockRouter.navigate).toHaveBeenCalledWith(["404"]);
    });
  });
});
