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

import { ILocationType, LocationType } from "../location-type.model";
import { LocationTypeService } from "../service/location-type.service";

import { LocationTypeRoutingResolveService } from "./location-type-routing-resolve.service";

describe("LocationType routing resolve service", () => {
  let mockRouter: Router;
  let mockActivatedRouteSnapshot: ActivatedRouteSnapshot;
  let routingResolveService: LocationTypeRoutingResolveService;
  let service: LocationTypeService;
  let resultLocationType: ILocationType | undefined;

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
    routingResolveService = TestBed.inject(LocationTypeRoutingResolveService);
    service = TestBed.inject(LocationTypeService);
    resultLocationType = undefined;
  });

  describe("resolve", () => {
    it("should return ILocationType returned by find", () => {
      // GIVEN
      service.find = jest.fn((id) => of(new HttpResponse({ body: { id } })));
      mockActivatedRouteSnapshot.params = { id: 123 };

      // WHEN
      routingResolveService
        .resolve(mockActivatedRouteSnapshot)
        .subscribe((result) => {
          resultLocationType = result;
        });

      // THEN
      expect(service.find).toBeCalledWith(123);
      expect(resultLocationType).toEqual({ id: 123 });
    });

    it("should return new ILocationType if id is not provided", () => {
      // GIVEN
      service.find = jest.fn();
      mockActivatedRouteSnapshot.params = {};

      // WHEN
      routingResolveService
        .resolve(mockActivatedRouteSnapshot)
        .subscribe((result) => {
          resultLocationType = result;
        });

      // THEN
      expect(service.find).not.toBeCalled();
      expect(resultLocationType).toEqual(new LocationType());
    });

    it("should route to 404 page if data not found in server", () => {
      // GIVEN
      jest
        .spyOn(service, "find")
        .mockReturnValue(
          of(new HttpResponse({ body: null as unknown as LocationType }))
        );
      mockActivatedRouteSnapshot.params = { id: 123 };

      // WHEN
      routingResolveService
        .resolve(mockActivatedRouteSnapshot)
        .subscribe((result) => {
          resultLocationType = result;
        });

      // THEN
      expect(service.find).toBeCalledWith(123);
      expect(resultLocationType).toEqual(undefined);
      expect(mockRouter.navigate).toHaveBeenCalledWith(["404"]);
    });
  });
});
