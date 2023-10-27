import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpHeaders, HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { of } from "rxjs";

import { UserLocationService } from "../service/user-location.service";

import { UserLocationComponent } from "./user-location.component";

describe("UserLocation Management Component", () => {
  let comp: UserLocationComponent;
  let fixture: ComponentFixture<UserLocationComponent>;
  let service: UserLocationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [UserLocationComponent],
    })
      .overrideTemplate(UserLocationComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(UserLocationComponent);
    comp = fixture.componentInstance;
    service = TestBed.inject(UserLocationService);

    const headers = new HttpHeaders();
    jest.spyOn(service, "query").mockReturnValue(
      of(
        new HttpResponse({
          body: [{ id: 123 }],
          headers,
        })
      )
    );
  });

  it("Should call load all on init", () => {
    // WHEN
    comp.ngOnInit();

    // THEN
    expect(service.query).toHaveBeenCalled();
    expect(comp.userLocations?.[0]).toEqual(
      expect.objectContaining({ id: 123 })
    );
  });
});
