import { TestBed } from "@angular/core/testing";
import {
  HttpClientTestingModule,
  HttpTestingController,
} from "@angular/common/http/testing";

import { ILocationType, LocationType } from "../location-type.model";

import { LocationTypeService } from "./location-type.service";

describe("LocationType Service", () => {
  let service: LocationTypeService;
  let httpMock: HttpTestingController;
  let elemDefault: ILocationType;
  let expectedResult: ILocationType | ILocationType[] | boolean | null;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    expectedResult = null;
    service = TestBed.inject(LocationTypeService);
    httpMock = TestBed.inject(HttpTestingController);

    elemDefault = {
      id: 0,
      name: "AAAAAAA",
    };
  });

  describe("Service methods", () => {
    it("should find an element", () => {
      const returnedFromService = Object.assign({}, elemDefault);

      service.find(123).subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "GET" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(elemDefault);
    });

    it("should create a LocationType", () => {
      const returnedFromService = Object.assign(
        {
          id: 0,
        },
        elemDefault
      );

      const expected = Object.assign({}, returnedFromService);

      service
        .create(new LocationType())
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "POST" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should update a LocationType", () => {
      const returnedFromService = Object.assign(
        {
          id: 1,
          name: "BBBBBB",
        },
        elemDefault
      );

      const expected = Object.assign({}, returnedFromService);

      service
        .update(expected)
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "PUT" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should partial update a LocationType", () => {
      const patchObject = Object.assign(
        {
          name: "BBBBBB",
        },
        new LocationType()
      );

      const returnedFromService = Object.assign(patchObject, elemDefault);

      const expected = Object.assign({}, returnedFromService);

      service
        .partialUpdate(patchObject)
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "PATCH" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should return a list of LocationType", () => {
      const returnedFromService = Object.assign(
        {
          id: 1,
          name: "BBBBBB",
        },
        elemDefault
      );

      const expected = Object.assign({}, returnedFromService);

      service.query().subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "GET" });
      req.flush([returnedFromService]);
      httpMock.verify();
      expect(expectedResult).toContainEqual(expected);
    });

    it("should delete a LocationType", () => {
      service.delete(123).subscribe((resp) => (expectedResult = resp.ok));

      const req = httpMock.expectOne({ method: "DELETE" });
      req.flush({ status: 200 });
      expect(expectedResult);
    });

    describe("addLocationTypeToCollectionIfMissing", () => {
      it("should add a LocationType to an empty array", () => {
        const locationType: ILocationType = { id: 123 };
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          [],
          locationType
        );
        expect(expectedResult).toHaveLength(1);
        expect(expectedResult).toContain(locationType);
      });

      it("should not add a LocationType to an array that contains it", () => {
        const locationType: ILocationType = { id: 123 };
        const locationTypeCollection: ILocationType[] = [
          {
            ...locationType,
          },
          { id: 456 },
        ];
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          locationTypeCollection,
          locationType
        );
        expect(expectedResult).toHaveLength(2);
      });

      it("should add a LocationType to an array that doesn't contain it", () => {
        const locationType: ILocationType = { id: 123 };
        const locationTypeCollection: ILocationType[] = [{ id: 456 }];
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          locationTypeCollection,
          locationType
        );
        expect(expectedResult).toHaveLength(2);
        expect(expectedResult).toContain(locationType);
      });

      it("should add only unique LocationType to an array", () => {
        const locationTypeArray: ILocationType[] = [
          { id: 123 },
          { id: 456 },
          { id: 30244 },
        ];
        const locationTypeCollection: ILocationType[] = [{ id: 123 }];
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          locationTypeCollection,
          ...locationTypeArray
        );
        expect(expectedResult).toHaveLength(3);
      });

      it("should accept varargs", () => {
        const locationType: ILocationType = { id: 123 };
        const locationType2: ILocationType = { id: 456 };
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          [],
          locationType,
          locationType2
        );
        expect(expectedResult).toHaveLength(2);
        expect(expectedResult).toContain(locationType);
        expect(expectedResult).toContain(locationType2);
      });

      it("should accept null and undefined values", () => {
        const locationType: ILocationType = { id: 123 };
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          [],
          null,
          locationType,
          undefined
        );
        expect(expectedResult).toHaveLength(1);
        expect(expectedResult).toContain(locationType);
      });

      it("should return initial array if no LocationType is added", () => {
        const locationTypeCollection: ILocationType[] = [{ id: 123 }];
        expectedResult = service.addLocationTypeToCollectionIfMissing(
          locationTypeCollection,
          undefined,
          null
        );
        expect(expectedResult).toEqual(locationTypeCollection);
      });
    });
  });

  afterEach(() => {
    httpMock.verify();
  });
});
